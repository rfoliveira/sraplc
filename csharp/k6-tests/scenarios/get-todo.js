import http from "k6/http";
import { sleep } from "k6";
import { Trend, Rate } from "k6/metrics";
import { check, fail } from "k6";

export let getTodoDuration = new Trend("get_todo_duration");
export let getTodoFailRate = new Rate("get_todo_fail_rate");
export let getTodoSuccessRate = new Rate("get_todo_success_rate");
export let getTodoRequests = new Rate("get_todo_requests");

export default function () {
  let res = http.get("http://localhost:5098/todo/2");

  getTodoDuration.add(res.timings.duration);
  getTodoRequests.add(1);
  getTodoFailRate.add(res.status == 0 || res.status > 399);
  getTodoSuccessRate.add(res.status < 399);

  let durationMsg = `Max duration ${4000 / 1000}s`;

  if (
    !check(res, {
      "max duration": (r) => r.timings.duration < 4000,
    })
  ) {
    fail(durationMsg);
  }

  sleep(1);
}
