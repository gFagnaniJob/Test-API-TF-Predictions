from flask import Flask, jsonify
from flask import request
from flask import abort
import base64

app = Flask(__name__)

tasks = [
    {
        'id': 1,
        'title': u'Buy groceries',
        'description': u'Milk, Cheese, Pizza, Fruit, Tylenol',
        'done': False
    },
    {
        'id': 2,
        'title': u'Learn Python',
        'description': u'Need to find a good Python tutorial on the web', 
        'done': False
    }
]

@app.route('/api/tasks', methods=['GET'])
def get_tasks():
    return jsonify({'tasks': tasks})

@app.route('/api/test', methods=['POST'])
def write_image():
    if not request.json or not 'img' in request.json or not 'name' in request.json:
        abort(400)
    name = request.json["name"]
    with open('received_images/' + name + '.jpg', 'wb') as f:
        imageBytes = request.json["img"]
        f.write(base64.decodebytes(imageBytes.encode()))
    f.close()
    response = "OK"
    return jsonify({"response": response})

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000)