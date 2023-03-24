import getTodos from "./scenarios/get-todos.js";
import getTodo from "./scenarios/get-todo.js";
import { group, sleep } from "k6";
// import postTodo from "./scenarios/post-todo.js";

export const options = {
  vus: 100,
  duration: "30s",
};

export default () => {
  group("CSharp Todo - GET Endpoints", () => {
    getTodos();
    getTodo();
  });

  // group("CSharp Todo - POST Endpoint", () => {
  //   postTodo();
  // });

  sleep(1);
};
