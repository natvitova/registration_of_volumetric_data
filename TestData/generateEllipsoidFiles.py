import matplotlib.pyplot as plt


def write_file(number, file):
    binary_representation = number.to_bytes(1, 'big')  # Convert integer to 4-byte binary representation (adjust the byte size as needed)
    file.write(binary_representation)


fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

fileMicro = open('testMicro.raw', 'wb')
# fileMacro = open('testMacro.raw', 'wb')

a = 8 # reflects on x axis
b = 6 # reflects on y axis
c = 5 # reflects on z axis

xIn = []
yIn = []
zIn = []

step = 0.5
const = 1/step

for x in range(0, int(a*const/2)): #for micro the upper bound should a*const/2 
    for y in range(0, b*const):
        for z in range(0, c*const):
            functionValue = ((x/const)**2)/(a**2) + ((y/const)**2)/(b**2) + ((z/const)**2)/(c**2)
            if (functionValue  <= 1):
                write_file(int(functionValue*200), fileMicro)
                xIn.append(x/const)
                yIn.append(y/const)
                zIn.append(z/const)
                # else:
                #     xOut.append(x/const)
                #     yOut.append(y/const)
                #     zOut.append(z/const)
                    
                # write_file(100, fileMacro)
            else:  #not inside the elipsoid
                # write_file(10, fileMacro)

                write_file(255, fileMicro)
            

ax.scatter(xIn, yIn, zIn, c='r', marker='o')
# ax.scatter(xOut, yOut, zOut, c='b', marker='o')

ax.set_xlabel('X Label')
ax.set_ylabel('Y Label')
ax.set_zlabel('Z Label')

ax.set_xlim(-a, a)
ax.set_ylim(-b, b)
ax.set_zlim(-c, c)

plt.title("Elipsoid")

plt.show()

fileMicro.close()
# fileMacro.close()