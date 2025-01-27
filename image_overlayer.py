from rich import print
from PIL import Image, ImageDraw
from tkinter.filedialog import askopenfilenames, askopenfilename

# Config
location = (90, 91)    #px
rectangle_location = [(location[0] - 2, location[1] - 2), (location[0] + 33, location[1] + 33)]

fill_colour = '#001626'
line_colour = '#00c1ff'

# Select Flag to Overlay
stamp = Image.open(askopenfilename())

# Select Images to Watermark
thumbnails = askopenfilenames()

# Loop through thumbnails and stampy
for file in thumbnails:
    # Open File
    print(f"Opening {file}...")
    thumb = Image.open(file)

    # Save a .old just in case
    thumb.save(file + '.old', 'PNG')

    # Add a Border
    print(f"Drawing border at {rectangle_location}...")
    drawing = ImageDraw.Draw(thumb)
    drawing.rounded_rectangle(rectangle_location, radius=3, fill=fill_colour, outline=line_colour)

    # Stamp the Flag
    thumb.paste(stamp, location, stamp)
    # thumb.show()

    thumb.save(file, 'PNG')
    print("File saved!\n\n")
