# Setting up an ASP.NET MVC App with GDS Precompiled Assets

This guide will walk you through setting up an ASP.NET MVC app with GDS precompiled assets.

## Prerequisites
1. An up-to-date version of .NET Core installed on your machine. [This tutorial will use .NET 8.0 SDK.](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. JetBrains Rider or Visual Studio installed on your machine. This guide will use [JetBrains Rider](https://www.jetbrains.com/rider/) as we primarily develop on macOS.
3. Clone this repository to your local machine.

## Step 1: Create a new ASP.NET MVC project

1. Navigate to the directory of the cloned repo on your local machine.
2. Open JetBrains Rider and select "New Solution".
3. On the left of the new solution setup, select "Web".
4. Under "Template", select "Web App (Model-View-Controller)".
5. Add a Solution Name and Project Name.
6. Make sure the "Target Framework" is set to "net8.0".
7. Check "Put Solution and Project in the same directory".
8. Click "Create".

## Step 2: Commit initial project setup to repo

1. Make sure you are up to date with `origin/main` by running `git pull origin main`.  
    You can merge `origin/main` into your local main branch by running `git merge origin/main`.
2. Stage your changes with `git add *`.
3. Commit your changes with `git commit -m "Initial project setup"`.
4. Push your changes to the remote repository with `git push`.

## Step 3: Add xUnit Project to solution
1. Right click the solution folder at the top of the solution explorer
2. Click ‘Add’, then ‘New Project’
3. Then click ‘Unit Test’ as Project Type
4. As Type select ‘xUnit’ in the dropdown
5. Make sure that the Target Framework is net8.0
6. Give your test project a name
7. Make sure that it is being created in the correct project directory
8. Once is is created, go to the Web App Project directory and find the Test folder than was created in it
9. Right click the folder and select ‘Exclude’
10. Build and run the Web App to make sure it is working
11. Then run your tests by right clicking on it and selecting run tests

## Step 4: Add GDS precompiled assets to your project

For a full tutorial, see the GOV.UK Guide on using GDS precompiled assets [here](https://frontend.design-system.service.gov.uk/install-using-precompiled-files/#try-gov-uk-frontend-using-precompiled-files).  
Note: This will require some tinkering to adapt it to the structure of a .NET Core project. Mainly in the way the assets are referenced in the `_Layout.cshtml` file and how you add assets to `wwwroot`.


1. Download the latest pre-compiled GDS assets from [this repo](https://github.com/alphagov/govuk-frontend/releases/tag/v5.7.1).  
   Extract the contents of the downloaded zip file.
2. Copy the `assets` folder from the extracted zip and paste this into the `wwwroot` folder of your project.
3. Create a folder under `wwwroot` called `stylesheets`.  
   Copy the `govuk-frontend-5.7.1.min.css` and `govuk-frontend-5.7.1.min.css.map` files from the extracted zip and paste these into the `stylesheets` folder.
4. Create a folder under `wwwroot` called `javascripts`.  
   Copy the `govuk-frontend-5.7.1.min.js` and `govuk-frontend-5.7.1.min.js.map` files from the extracted zip and paste these into the `javascripts` folder.

## Step 5: Update your layout file to include GDS assets

1. Under `Views > Shared` in your project, open the `_Layout.cshtml` file.
2. In the `<head></head>` section of the file, add the following code:   
   `<link rel="stylesheet" href="~/stylesheets/govuk-frontend-5.7.1.min.css">`
3. At the top of `<body>`, inside the `<body>` tags, add the following code:  
    ```
    <script>
       document.body.className += ' js-enabled' + ('noModule' in HTMLScriptElement.prototype ? ' govuk-frontend-supported' : '');
    </script>
   ```
4. At the bottom of `<body>`, whilst inside the `<body>` tags, just before the `@RenderBody()` call, add the following code:  
    ```
   <!-- Import the minified GOV.UK JS here -->
    <script type="module" src="~/javascripts/govuk-frontend-5.7.1.min.js"></script>
    <script type="module">
        import { initAll } from './javascripts/govuk-frontend-5.7.1.min.js' // Ignore the error/warning here, it is grabbing it just fine.
        initAll()
    </script>
   ```
   
Your project now has access to the Government Design System Precompiled Assets via the `_Layout.cshtml` file.

## Step 6: Commit changes to repo

1. Stage your changes with `git add *`.
2. Commit your changes with `git commit -m "Added GDS precompiled assets to project"`.
3. Push your changes to the remote repository with `git push`.
4. Create a pull request to merge your changes into the main branch if necessary.

## Step 7: Working with GDS Components

1. See the following page for info on working with GDS components.  
   Note: You will have to copy and paste the HTML examples as nunjucks is not compatible/configured here.
    [GOV.UK Design System Components](https://design-system.service.gov.uk/components/)
2. Here is a link to an example repo: [Example GDS Repo](https://github.com/ntotten-kainos/GDS-Example-Repo-MVC-DOTNET)

# How to Lint Locally

## Prerequisites 

### Make sure Rider is configured to use .editorconfig (On Mac):
    1. Click on 'JetBrains Rider' in the top left of the screen between the Apple logo and 'File'
    2. Click 'Settings...'
    3. Go to 'Editor' -> 'CodeStyle'
    4. Ensure 'Enable EditorConfig Support' is checked

### Run the command `npm install -g editorconfig-checker` to install editorconfig-checker globally
```bash
npm install -g editorconfig-checker
````

### Run the command `editorconfig-checker` to check code for linting errors
```bash
editorconfig-checker 
```

# Adding credentials for seeded/default users
Add the following lines to your environment variable config of choice, e.g. your `.zshrc`:

```bash
export GYJ_ADMIN_PASSWORD="<your password here>"
export GYJ_APPLICANT_PASSWORD="<your password here>"
export GYJ_RECRUITER_PASSWORD="<your password here>"
```

and save/reload as needed.
The program will pick up these environment variables on start-up and seed the database with users with the correct roles if they do not already exist.

