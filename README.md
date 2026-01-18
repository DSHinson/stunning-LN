# Angular Project Setup Instructions

## Navigate to Angular Project

Navigate to the Angular project directory:

```bash
cd LexisNexis\LexisNexisFrontEnd\LexisNexis.Client
```

## 1. Node + NPM Setup

Install and use Node.js version 24.3.0:

```bash
nvm install 24.3.0
nvm use 24.3.0
```

## 2. Update NPM

Update npm to version 11.7.0:

```bash
npm install -g npm@11.7.0
```

## 3. Install Angular CLI

Install Angular CLI globally:

```bash
npm install -g @angular/cli@20.3.14
```

## 4. Install Project Dependencies

Install project dependencies from package-lock.json:

```bash
npm ci
```

## 5. Configure Proxy Settings

Update the proxy configuration file to point at your server API IP. Edit `proxy.conf.json`:

```json
{
  "/api": {
    "target": "https://localhost:7059",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
}
```

Replace `https://localhost:7059` with your actual server API IP address.

## 6. Start Development Server

Start the Angular development server:

```bash
ng serve
```

---

## Running the .NET API

### Prerequisites

Ensure you have **.NET 8 SDK** installed. All projects in this solution target .NET 8.

You can verify your installation by running:

```bash
dotnet --version
```

### Open the Solution in Visual Studio

Navigate to and open the solution file located at:

```
stunning-LN\LexisNexisServer\LexisNexis.slnx
```

1. Open Visual Studio
2. File → Open → Project/Solution
3. Browse to the path above and select `LexisNexis.slnx`

### Restore NuGet Packages

This project uses **Central Package Management**. Visual Studio should automatically restore packages when you open the solution. If packages are not restored automatically:

- Right-click on the solution in Solution Explorer
- Select **Restore NuGet Packages**

Alternatively, you can restore packages via the command line:

```bash
dotnet restore
```

### Set Startup Project

Ensure **LexisNexis.API** is set as the startup project:

1. In Solution Explorer, right-click on **LexisNexis.API**
2. Select **Set as Startup Project**

(The startup project will appear in bold in Solution Explorer)

### Run the API

Press **F5** or click the **Run** button to start the API server.

---

**Note:** After running `ng serve`, your application will typically be available at `http://localhost:4200/`