import cv2
import pickle
import numpy as np
from keras.models import load_model
def detectSymbol():
  image_number = 0
  symbol_order = []
  while image_number < 25:
      
      img=cv2.imread('ROI_{}.png'.format(image_number))
      
      
      def preProcess(img):   
        img = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
        img = cv2.equalizeHist(img)
        img = img /255.0
        return img   
      
      pickle_in = open("model_trained_new.p","rb")
      model = pickle.load(pickle_in)
      pickle_in.close()
      frame = img 
      
      
      
      
      if True:
          img = np.asarray(frame)
          img = cv2.resize(img, (32,32))
          img = preProcess(img)
      
          img = img.reshape(1,32,32,1)
      

          
        # predict
          classIndex = int(model.predict(img))
          predictions = model.predict(img)
          probVal = np.amax(predictions)
          print(classIndex, 'ROI_{}.png'.format(image_number) )
          symbol_order.append(classIndex)
          image_number += 1
  return symbol_order


detectSymbol()





    
 

        
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    