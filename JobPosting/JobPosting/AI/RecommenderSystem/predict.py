import sys
import numpy as np
import csv
import matplotlib.pyplot as plt
import pickle as pck
from utils import get_parameters, sigmoid, sigmoid_backward, softmax, softmax_backward, convert_to_oh, convert_dictionary_to_vector, convert_vector_to_dictionary, initialize_parameters, add_parameters, subtract_parameters
from Train import model, predict

if __name__ == "__main__":
    n_y = pck.load(open(r"C:\Users\pevip\OneDrive\Documents\GitHub\JobSite\JobPosting\JobPosting\AI\RecommenderSystem\prevlayer.p", "rb"))
    n_y = int(n_y)
    layers_dim = [2,6,10,n_y]
    user = int(sys.argv[1])
    prev1 = int(sys.argv[2])
    prev2 = int(sys.argv[3])
    userName = sys.argv[4]
    X = np.array([prev1, prev2], dtype=np.int).reshape((2,1))
    parameters = get_parameters(layers_dim, userName, user, C = n_y)
    Y = predict(X, user, parameters, layers_dim)
    print (Y)
