from flask import Flask, request, make_response
import json
from PIL import Image
from daire_croplama_ve_hough_transform.cropCircle import *
from daire_croplama_ve_hough_transform.findCricle import *
from tas_sembolü_tespit_etme.detect_symbol import *

import time
    

app = Flask(__name__)

@app.route('/upload_image', methods=['POST'])
def upload_image():
    """
    image = request.files['image']
    image.save("image.png")
    cropCirle()
    findCircle()
    symbolList = detectSymbol()

    processed_data = {"data": symbolList}
    print(symbolList)
    response = make_response(json.dumps(processed_data))
    response.headers["Content-Type"] = "application/json"
    """
    time.sleep(2)
    return "response"


if __name__ == '__main__':
    app.run(debug=True)
