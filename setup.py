"""
Setup script for Hawkmatix Regression.
"""

# Import modules.
import getpass
import os

# Check if the OS is Windows.
if os.name is not "nt":
    raise OSError("This operating system is not supported.")

# Define the file locations.
project_name = "Regression"
file_name = "Hawkmatix" + project_name.replace(' ', '') + ".cs"
here = os.path.abspath(os.path.dirname(__file__))
path_to_ind = os.path.join(here, project_name.lower().replace(' ', '_'),
    file_name)
path_to_nt = os.path.join("C:", "Users", getpass.getuser(), "Documents",
    "NinjaTrader 7", "bin", "Custom", "Indicator")

# If the NinjaTrader directory exists put the source file there.
if os.path.isdir(path_to_nt):
    os.rename(path_to_ind, os.path.join(path_to_nt, file_name))
else:
    print("Install failed: Could not find NinjaTrader path.")

# Now the user must compile from the NinjaScript Editor.
print("To fully install the script please follow the following directions.")
print("Open NinjaTrader.")
input("Press Enter to continue...\n")
print("In the Control Center go to Tools > Edit NinjaScript > Indicator... \
and open any indicator from the list.")
input("Press Enter to continue...\n")
print("Press the Compile button in the top menu bar.")
input("Press Enter to continue...\n")
print("Once the compilation completes without errors, the add-on has been \
installed.")
