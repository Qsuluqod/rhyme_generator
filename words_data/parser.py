input_file = open("P:\Ostatní Programování\student-prokoma1\slova_data\capek1.csv", "r", encoding="utf-8")
output_file = open("P:\Ostatní Programování\student-prokoma1\slova_data\data.txt", "w", encoding="utf-8")

allowed = "aábcčdďeéěfghiíjklmnňoópqrřsštťuúůvwxyýzž"
not_allowed = ".\",;0123456789?!@#$%^&*()-+*/"
count = 0

def ilegal(word):
    global count
    for letter in word:
        if letter in not_allowed:
            print(letter)
            print(word)
            count += 1
            return True
    return False

def legal(word):
    global count
    for letter in word:
        if letter not in allowed:
            print(letter)
            print(word)
            count += 1
            return False
    return True

first = True
line = input_file.readline()

while line:
    try:
        word = line.split(";")[1][1:-1].strip().lower()
    except IndexError:
        line = input_file.readline()
        continue

    if legal(word) and not ilegal(word) and word:
        if first:
            first = False
        else:
            output_file.write(",")
        output_file.write(word)

    line = input_file.readline()

input_file.close()
output_file.close()
print(count)