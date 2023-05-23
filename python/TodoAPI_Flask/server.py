from flask import Flask
from flask_restful import Resource, Api
from sqlalchemy import create_engine
from json import dumps

db_connect = create_engine("sqlite://python.db")
app = Flask(__name__)
api = Api(app)

class Todos(Resource):
    def get(self):
        conn = db_connect.connect()
        query = conn.execute("select * from todo")
        result = [dict(zip(tuple(query.keys()), i)) for i in query.cursor]
        return jsonify(result)
    
    def post(self):
        conn = db_connect.connect()
        description = request.json["description"]
        completed = request.json["completed"]
        
        conn.execute(
            "insert into todo (description, completed) values ('{0}',{1})".format(description, completed)
        )
        
        query = conn.execute("select * from todo order by id desc limit 1")
        result = [dict(zip(tuple(query.keys()), i)) for i in query.cursor]
        return jsonify(result)
    
    def put(self):
        conn = db_connect.connect()
        id = request.json["id"]
        description = request.json["description"]
        completed = request.json["completed"]
        
        conn.execute(
            "update todo set description = '" + str(description) + "', " +
            "completed = " + bool(completed) + " " + "where id = %d" % int(id)
        )
        
        query = conn.execute("select * from todo where id = %id" % int(id))
        result = [dict(zip(tuple(query.keys()), i)) for i in query.cursor]
        return jsonify(result)
    
class TodoById(Resource):
    def delete(self, id):
        conn = db_connect.connect()
        conn.execute("delete from todo where id = %d" % int(id))
        return {"status": "success"}
    
    def get(self, id):
        conn = db_connect.connect()
        query = conn.execute("select * from todo where id = %d" % int(id))
        result = [dict(zip(tuple(query.keys()), i)) for i in query.cursor]
        return jsonify(result)
    
api.add_resource(Todos, "/todos")
api.add_resource(TodoById, "/todos/<id>")

if __name__ == "__main__":
    app.run()