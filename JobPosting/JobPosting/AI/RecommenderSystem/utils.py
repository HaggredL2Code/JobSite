import numpy as np
import pickle as pck

def read_csv(file):
    with open(file) as csvfile:
        parameters = {}
        reader = csv.DictReader(csvfile)
        first = True
        count = 1
        for row in reader:
            parameters["W" + str(count)] = row["W" + str(count)]
            parameters["b" + str(count)] = row["b" + str(count)]
            count += 1

        return parameters

def get_parameters(layers_dim, userName, user, C):
    parameters = []
    flag = False
    L = len(layers_dim)
    with open(r"C:\Users\pevip\OneDrive\Documents\GitHub\JobSite\JobPosting\JobPosting\AI\RecommenderSystem\all_users.csv", "r+") as f:
        for l in f:
            l = np.array(l, dtype=np.int)
            if user in l:
                prevLayer = pck.load(open(r"C:\Users\pevip\OneDrive\Documents\GitHub\JobSite\JobPosting\JobPosting\AI\RecommenderSystem\prevlayer.p", "rb"))
                parameters = pck.load(open("C:/Users/pevip/OneDrive/Documents/GitHub/JobSite/JobPosting/JobPosting/AI/RecommenderSystem/users/" + userName + "_profile.p", "rb"))
                if int(prevLayer) < C:
                    parameters = add_parameters(np.abs(C - prevLayer), parameters, layers_dim)
                elif int(prevLayer) > C:
                    parameters = subtract_parameters(np.abs(C - prevLayer), parameters, layers_dim)
                flag = True
                break
        if not flag:
            parameters = initialize_parameters(L, layers_dim)
            f.write(str(user) + "\n")

    return parameters

def sigmoid(Z):
    return 1/(1 + np.exp(-Z))

def sigmoid_backward(dA, Z):
    s = 1/(1 + np.exp(-Z))
    dZ = dA * s * (1 - s)
    return dZ

def softmax(Z):
    exps = np.exp(Z - np.max(Z))
    return exps / np.sum(exps)

def softmax_backward(AL, Y):
    dZ = AL - Y
    return dZ

def convert_to_oh(Y_train, C=4):
    Y = np.eye(C)[Y_train.reshape(-1)]
    return Y

def convert_dictionary_to_vector(dictionary):
    row = dictionary.shape[0] # 6
    column = dictionary.shape[1] # 2
    vector = np.zeros(row * column) # 12
    start = 0
    for c in range(column):
        temp = dictionary[:,c] # (6,1)
        end = row * (c + 1)
        vector[start : end] = temp
        start = end


    return np.array(vector, dtype = np.float)

def convert_vector_to_dictionary(vector, layers_dim, l):
    row = layers_dim[l+1]
    column = layers_dim[l]
    dictionary = []
    start = 0
    for c in range(column):
        end = row * (c + 1)
        temp = vector[start:end]
        print (temp)
        start = end
        dictionary.append(temp)

    return np.array(dictionary, dtype=np.float).T


def initialize_parameters(n_a, layers_dim):
    parameters = {}
    for n in range(n_a-1):
        parameters["W" + str(n+1)] = np.random.randn(layers_dim[n+1], layers_dim[n]) * np.sqrt(2/(layers_dim[n+1] + layers_dim[n]))
        parameters["b" + str(n+1)] = np.ones((layers_dim[n+1],1))

    return parameters

def add_parameters(sn, parameters, layers_dim):
    L = len(layers_dim)
    for i in range(sn):
        parameterWToAdd = (np.random.randn(layers_dim[-2]) * np.sqrt(2/layers_dim[-1] + layers_dim[-2])).reshape((1,layers_dim[-2]))
        parameterbToAdd = np.ones((1,1))
        parameters["W" + str(L-1)] = np.append(parameters["W" + str(L-1)],parameterWToAdd, axis=0)
        parameters["b" + str(L-1)] = np.append(parameters["b" + str(L-1)],parameterbToAdd, axis=0)

    return parameters

def subtract_parameters(sn, parameters, layers_dim):
    L = len(layers_dim)
    for i in range(sn):
        parameters["W" + str(L - 1)] = np.delete(parameters["W" + str(L - 1)], -1, axis=0)
        parameters["b" + str(L - 1)] = np.delete(parameters["b" + str(L - 1)], -1, axis=0)

    return parameters
