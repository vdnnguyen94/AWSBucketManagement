# AWS S3 Management Tool (WPF)

![Language](https://img.shields.io/badge/Language-C%23-blue.svg) ![Framework](https://img.shields.io/badge/Framework-.NET%208-blueviolet.svg) ![Platform](https://img.shields.io/badge/Platform-WPF-informational.svg) ![Service](https://img.shields.io/badge/Service-AWS%20S3-orange.svg)

A desktop application built with C# and WPF that provides a user-friendly interface for managing Amazon S3 buckets and objects. The application allows for full CRUD (Create, Read, Update, Delete) operations on both buckets and the objects within them.


---
## Features

### Bucket Management
- **List Buckets**: View all S3 buckets in your account along with their creation dates.
- **Create Buckets**: Create new S3 buckets directly from the application.
- **Delete Buckets**: Delete buckets. The application includes a safety check that asks for confirmation if the bucket is not empty.

### Object Management
- **List Objects**: Select a bucket to view all objects it contains, including their names (keys) and sizes.
- **Upload Objects**: Browse for local files and upload them to a selected S3 bucket. The object list refreshes automatically.
- **Download Objects**: Select an object from the list and download it to your local machine, choosing the save location via a file dialog.
- **Delete Objects**: Delete specific objects from a bucket with a confirmation prompt.

### UI/UX
- **Multi-Window Navigation**: A clean main menu navigates to separate, dedicated windows for bucket and object operations.
- **Consistent Layout**: Windows always open in the center of the screen for a smooth user experience.

---
## Technology Stack
- **C# & .NET 8**: The core language and framework for the application logic.
- **WPF (Windows Presentation Foundation)**: Used to build the graphical user interface.
- **AWS SDK for .NET**: The official AWS library used to interact with the S3 API.
- **DotNetEnv**: A library to manage environment variables for storing AWS credentials securely.

---
## Prerequisites

Before running the application, you need an AWS account and an IAM User with the appropriate permissions.

1.  **AWS Account**: You must have an active AWS account.
2.  **IAM User**: For security, do not use your root account credentials. Create a new **IAM User** in the AWS Console.
3.  **Permissions**: Attach the **\`AmazonS3FullAccess\`** managed policy to your IAM User. This gives the user all the necessary permissions to manage S3 buckets and objects.
4.  **Access Keys**: After creating the user, generate an **Access Key ID** and a **Secret Access Key**. These are the credentials the application will use.

---
## Getting Started

### 1. Clone the Repository
\`\`\`bash
git clone [https://github.com/vdnnguyen94/AWSBucketManagement.git](https://github.com/vdnnguyen94/AWSBucketManagement.git)
\`\`\`

### 2. Configure Environment Variables
This project uses a \`.env\` file to handle your AWS credentials securely.

- Create a new file named **\`.env\`** in the root directory of the project (the same folder as your \`.csproj\` file).
- Open the \`.env\` file and add your credentials in the following format, replacing the placeholder text with your actual keys.

\`\`\`plaintext
# .env file example
AWS_ACCESS_KEY_ID=YOUR_ACCESS_KEY_ID
AWS_SECRET_ACCESS_KEY=YOUR_SECRET_ACCESS_KEY
AWS_REGION=us-east-1
\`\`\`
**Important**: In Visual Studio, you must set the properties for the \`.env\` file. Click the file in the Solution Explorer, go to the **Properties** window, and change **Copy to Output Directory** to **Copy if newer**.

### 3. Build and Run
Open the solution (\`.sln\`) file in Visual Studio and run the project (press F5 or the Start button).
