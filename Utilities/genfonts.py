import sys

def getLines(filename):
	with open(filename, "rt") as f:
		return f.readlines()

class Letter:
	def __init__(self):
		self.name = ""
		self.lines = []
		self.verts = []
	def collectVerts(self, vertData):
		newLines = []
		vertDictionary = {}

		def processVertex(vertIndex):
			if vertIndex in vertDictionary:
				return vertDictionary[vertIndex]
			newIndex = len(self.verts)
			self.verts.append(vertData[vertIndex - 1])
			vertDictionary[vertIndex] = newIndex
			return newIndex

		for line in self.lines:
			newLines.append((processVertex(line[0]), processVertex(line[1])))
		self.lines = newLines

def removeJunkFromName(name):
	pos = name.rfind('_')
	if pos < 0:
		return name
	return name[:pos]

def processFile(filename, outFilename, className):
	objLines = getLines(filename)
	verts = []

	curObjects = [Letter()]
	objects = []

	for line in objLines:
		curObject = curObjects[0]
		contents = line.split()
		if not contents:
			continue

		keyword = contents[0]
		if keyword == "#":
			continue

		if keyword == "v":
			newVert = tuple(float(x) for x in contents[1:-1])
			#print newVert
			verts.append(newVert)
			continue

		if keyword == "o":
			tmpObj = Letter()
			objects.append(tmpObj)
			curObjects[0] = tmpObj
			tmpObj.name = removeJunkFromName(contents[1])
			continue

		if keyword == "l":
			line = contents[1:]
			line = tuple(int(x) for x in line)
			#print line
			curObject.lines.append(line)
			continue

	for obj in objects:
		obj.collectVerts(verts)
		print obj.name
		print obj.lines
		print obj.verts


	objects = sorted(objects, key=lambda(x): x.name)

	with open(outFilename, "wt") as outFile:
		for curObj in objects:
			floatData = "new float[]{{{0}}}".format(", ".join(["{0}f".format(x) for y in curObj.verts for x in y]))
			lineData = "new int[]{{{0}}}".format(", ".join([str(x) for y in curObj.lines for x in y]))
			line = "\t\tcharMap['{0}'] = new {3}({1}, {2});\n".format(curObj.name, floatData, lineData, className)
			outFile.write(line)
		pass


if len(sys.argv) > 3:
	processFile(sys.argv[1], sys.argv[2], sys.argv[3])

