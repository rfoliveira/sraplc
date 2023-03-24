import http from "k6/http";
import { Trend, Rate } from "k6/metrics";
import { Task } from "./taskModel.js";
import { check, fail } from "k6";

export let postTodoDuration = new Trend("post_todo_duration");
export let postTodoFailRate = new Rate("post_todo_fail_rate");
export let postTodoSuccessRate = new Rate("post_todo_success_rate");
export let postTodoRequests = new Rate("post_todo_requests");

export default function () {
  let taskNumber = parseInt(Math.random() * (1000 - 0) + 0);
  let isCompleted = !!parseInt(Math.random() * (2 - 0) + 0);
  let task = new Task(null, `Do #${taskNumber} task`, isCompleted);

  let res = http.post("http://localhost:5098/todo", task);

  postTodoDuration.add(res.timings.duration);
  postTodoRequests.add(1);
  postTodoFailRate.add(res.status == 0 || res.status > 399);
  postTodoSuccessRate.add(res.status != 201);

  let durationMsg = `Max duration ${5000 / 1000}s`;

  if (
    !check(res, {
      "max duration": (r) => r.timings.duration < 5000,
    })
  ) {
    fail(durationMsg);
  }
}
