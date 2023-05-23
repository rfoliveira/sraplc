from flask import Flask
from flask_restful import reqparse, abort, Api, Resource

app = Flask(__name__)
api = Api(app)


todos = {
    '1': {'task': 'build an API'},
    '2': {'task': '?????'},
    '3': {'task': 'profit!'}
}


def abort_if_todo_doesnt_exist(id):
    if id not in todos:
        abort(404, message=f'Todo ID {id} not found')


parser = reqparse.RequestParser()
parser.add_argument("task")


class TodoById(Resource):
    def get(self, id):
        abort_if_todo_doesnt_exist(id)
        return todos[id]
    
    def put(self, id):
        abort_if_todo_doesnt_exist(id)
        
        args = parser.parse_args()
        task = {"task": args["task"]}
        todos[id] = task
                
        return task
    
    def delete(self, id):
        abort_if_todo_doesnt_exist(id)
        del todos[id]
        return "", 204
    

class Todos(Resource):
    def get(self):
        return todos
    
    def post(self):
        args = parser.parse_args()
        task = {"task": args["task"]}
        
        id = int(max(todos.keys())) + 1
        todo_id = f"{id}"
        
        todos[todo_id] = task
        return task, 201
        

api.add_resource(TodoById, "/todos/<id>")
api.add_resource(Todos, "/todos")

if __name__ == "__main__":
    app.run(debug=True)
