import csv
import os


def writeToCsv(newElement, fileName):
    columns = ["name", "cal", "prot", "fat", "carb"]
    if os.path.exists(fileName):
        with open(fileName, "a") as file:      
            writer = csv.DictWriter(file, fieldnames = columns)              
            writer.writerow(newElement)
    else:
        with open(fileName, "w", newline="") as file:
            writer = csv.DictWriter(file, fieldnames = columns)              
            writer.writeheader()
            writer.writerow(newElement)


def getAllProductNames(fileName):
    with open(fileName, "r", newline="") as file:
        reader = csv.DictReader(file)
        allProductNames = list()        
        for row in reader:
            # print(row["name"], " : ", row["cal"], ", ", row["prot"], ", ", row["fat"], ", ", row["carb"])
            allProductNames.append(row["name"])
        return allProductNames           


def findNamesByStartWith(fileName, letters):
    with open(fileName, "r", newline="") as file:
        reader = csv.DictReader(file)
        allMatches = list()
        for row in reader:                  
            if row["name"].startswith(letters):
                allMatches.append(row["name"])
        return allMatches
    
def findProduct(fileName, productName):
    with open(fileName, "r", newline="") as file:
        reader = csv.DictReader(file)        
        for row in reader:                  
            if row["name"].lower() == productName.lower():
                return row
                
        


path = r"c:\tatsiana\projects\FitMeApp\FitMeApp\wwwroot\Csv\Products.csv"
# product = {"name": "plum", "prot": 0.8, "fat": 0.3, "carb": 9.6}
# writeToCsv(product, path)
#readFromCsv(path)
findBy = "veg"
result = findNamesByStartWith(path, findBy)
#product = findProduct(path, "Banana")
#print(product["name"], " : ", product["cal"], ", ", product["prot"], ", ", product["fat"], ", ", product["carb"])
