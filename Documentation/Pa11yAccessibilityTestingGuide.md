#  Pa11y Accessibility testing guide

**What you will need:** Node.js and npm on your MacBook.

Run this command in project root to install Pa11y-CI: **npm install --save-dev pa11y-ci**

Create a File called **.pa11yci** in the root of project for configuration

An example of a **.pa11yci** file to show format of automated testing:
```json
{

    "defaults": {

    "timeout": 30000,

    "standard": "WCAG2AA"

},

    "urls": [

        "<http://localhost:5000>",

        "<http://localhost:5000/page1>",

        "<http://localhost:5000/page2>"

    ]

}
```
For easier testing we can add this to the **package.json** :
```json
"scripts": {

    "test:accessibility": "pa11y-ci"

}
```
To run the tests manually start the instance of our app and then use this command to run the checks: **npm run test:accessibility** this will trigger the script we made above

Integrating pa11y with **GitHub actions:** pa11y.yml to the workflows folder
```yml
name: Accessibility Testing
    
on:
    push:
        branches:
        - main
    pull_request:
        branches:
        - main

jobs:
    accessibility:
    runs-on: ubuntu-latest
    steps:
        - name: Checkout code
          uses: actions/checkout@v2

        - name: Set up Node.js
          uses: actions/setup-node@v2
          with:
            node-version: '16'

        - name: Install Pa11y CI
           run: npm install --save-dev pa11y-ci

        - name: Start .NET server
           run: |
            dotnet restore
            dotnet build
            dotnet run &
            sleep 15

        - name: Run Pa11y CI
           run: npx pa11y-ci
```