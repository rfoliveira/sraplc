import http from "k6/http";
import { sleep } from "k6";
import { Trend, Rate } from "k6/metrics";
import { check, fail } from "k6";

export let getTodosDuration = new Trend("get_todos_duration");
export let getTodosFailRate = new Rate("get_todos_fail_rate");
export let getTodosSuccessRate = new Rate("get_todos_success_rate");
export let getTodosRequests = new Rate("get_todos_requests");

export default function () {
  let res = http.get("http://localhost:5098/todo");

  getTodosDuration.add(res.timings.duration);
  getTodosRequests.add(1);
  getTodosFailRate.add(res.status == 0 || res.status > 399);
  getTodosSuccessRate.add(res.status < 399);

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
