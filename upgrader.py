import pkg_resources
from subprocess import call

packages = [dist.project_name for dist in pkg_resources.working_set]

for package in packages:
    if package == 'pip':
        continue

    print("Upgrading:", package)

    call("pip install " + package + " --upgrade", shell=True)

call("pip freeze > requirements.txt", shell=True)
