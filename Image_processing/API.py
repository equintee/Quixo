from flask import Flask, request, make_response
import json
from PIL import Image
from daire_croplama_ve_hough_transform.cropCircle import *
from daire_croplama_ve_hough_transform.findCricle import *
    

app = Flask(__name__)

@app.route('/upload_image', methods=['POST'])
def upload_image():
    image = request.files['image']
    image.save("image.png")
    cropCirle()
    findCircle()
    #symbolList = detectSymbol()

    processed_data = {"data": [1,2,3,4,5]}
    response = make_response(json.dumps(processed_data))
    response.headers["Content-Type"] = "application/json"
    return response


if __name__ == '__main__':
    app.run(debug=True)
