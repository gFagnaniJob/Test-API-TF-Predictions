# API
# POST /api/image  ->  ricevo immagine e invio json con predictions
from flask import Flask, render_template, request, jsonify, json
from flask_uploads import UploadSet, configure_uploads, IMAGES
from werkzeug.utils import secure_filename
import getpass
import base64
#####SCRIPT CHE PERMETTE DI FAR PARTIRE LO SCRIPT QUANDO PARTE WINDOWS ########

# USER_NAME = getpass.getuser()


# def add_to_startup(file_path=""):
#     if file_path == "":
#         file_path = os.path.dirname(os.path.realpath((Server.py))
#     bat_path = r'C:\Users\%s\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup' % USER_NAME
#     with open(bat_path + '\\' + "open.bat", "w+") as bat_file:
#         bat_file.write(r'start "" %s' % file_path)

##### FINE SCRIPT ######

app = Flask(__name__)

@app.route('/api/predictions', methods=['POST'])
def add_message():
   
    data = request.get_json()
    if data is None:
        print("No valid request body, json missing!")
        return jsonify({'error': 'No valid request body, json missing!'})
    else:
        with open("imageToSave.jpg", "wb") as fh:
            img = request.json['img']
            fh.write(base64.decodebytes(img.encode()))
        fh.close()
    return jsonify({"response": "OK"})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
