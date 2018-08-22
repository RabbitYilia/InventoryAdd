# Intro
This is a go program to add new substance on Cambridgesoft Inventory 13 to avoid the orginal bug.

The bug will lead to error when adding.Or Cannot display the correct structure after adding.

# The Method
First You need to input CAS Number which is required.
if leave empty the program will exit.

then according to the CAS searching the infomation from the database from the Internet.

If cannot get any info from internet the program will ask you input manually.

Then calculate the needed info by using C# API

Ensure the info ,if there is nothing wrong with the default value,leave it empty to use default value.

After that  add the data into the database

# Useage Tips

1. Open MSSQL sa account and ip connect
2. Run this program

# Speacial Thanks

The lovely friends @ School of Chemistry and Chemical Engineering,Nanjing University.

National Center for Biotechnology Information, U.S. National Library of Medicine