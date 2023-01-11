import cv2 
import numpy as np
from collections import defaultdict
import sys
import matplotlib.pyplot as plt
import math

def cropCirle():
    # get image
    img = cv2.imread('image.png')
    
    # get width and height
    hei = 504
    wid = 672
    

    img = cv2.resize(img, (hei,wid))
    image = np.copy(img) 
    height, width = img.shape[:2]
    print("height",height )
    print("width",width )
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    gray_blurred = cv2.bilateralFilter(gray, 11, 30, 30)

    # tune circles size
    detected_circles = cv2.HoughCircles(gray_blurred,
                                        cv2.HOUGH_GRADIENT,1,
                                        param1=50,
                                        param2=30,
                                        minDist=1000,
                                        minRadius=150,
                                        maxRadius=252)

    if detected_circles is not None:
        # Convert the circle parameters a, b and r to integers.
        detected_circles = np.uint16(np.around(detected_circles))


        for pt in detected_circles[0, :]:
            a, b, r = pt[0], pt[1], pt[2]

        

        
        

    for y in range(height):
        for x in range(width):
            if (x - a) ** 2 + (y - b) ** 2 >= r ** 2:
                img[y][x] = [255, 255, 255]
                

    cv2.imwrite("cropCircle.png", img)
    cv2.imshow("Detected circles", img)
    cv2.waitKey(0)
    

