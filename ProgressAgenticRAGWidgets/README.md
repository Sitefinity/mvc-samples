Custom Progress Agentic RAG widgets
======

This guide demonstrates how to create and configure a set of custom widgets powered by Progress Agentic RAG.

## Available Widgets
- AI Assistant Chat
- AI Ask Box
- AI Answer
- AI Search Results

# Installing the widgets

Follow these steps to install the custom Agentic RAG widgets in your Sitefinity application:

1. Clone the [mvc-samples](https://github.com/Sitefinity/mvc-samples) repository.
2. Check Sitefinity NuGet versions - Ensure that the Sitefinity NuGet package versions used in the sample match those used in your project. If they differ, update the NuGet references in the widget project to match your Sitefinity version.
3. Build the desired widget's project.
4. Reference the DLLs - Add references to the generated DLLs in your Sitefinity web application.
5. Create a **Global.asax.cs** file - If your Sitefinity web application doesn’t already include one, create a Global.asax.cs file.
6. Configure the **Global.asax.cs** file - Modify its contents to match the sample located at: **{{PROJECT_NAME}}/SitefinityWebApp/Global.asax.cs**
7. Rebuild your Sitefinity web application - Once built, the new widgets will appear in your Sitefinity Page Toolbox.

# Configuring the Widgets

## Configure Nuclia Settings
	- Base URL
		- Log in to the Agentic RAG Dashboard.
		- Copy NucliaDB API endpoint
		- In Sitefinity, navigate to: **Administration → Settings → Advanced → AgenticRAG → Base URL**
		- Paste the copied endpoint without the path.

## Configure Knowledge Box
    - Knowledge box ID
		- Log in to the Agentic RAG Dashboard.
        - Copy the Knowledge Box UID.
		- In Sitefinity, navigate to: **Administration → Settings → Advanced → AgenticRAG → Knowledge Boxes -> KnowledgeBoxId**.
		- Paste the copied UID.

    - Knowledge box API key
		- In the Agentic RAG Dashboard, go to Advanced → API Keys.
        - Create a new API key and copy it.      
		- In Sitefinity, navigate to: **Administration → Settings → Advanced → AgenticRAG → Knowledge Boxes -> KnowledgeBoxKey**.
        - Paste the copied API key.

## Configure Assistant Settings
In Sitefinity, configure the following values under: **Administration → Settings → Advanced → AgenticRAG → Assistant**

| Setting                       | Value                                  |
| ----------------------------- | -------------------------------------- |
| **AdminApiBaseUrl**           | `https://api.sitefinity.cloud/Version` |
| **CdnHostName**               | `cdn.assistant.api.sitefinity.cloud`   |

## Configure Security Headers
- Navigate to **Administration → Settings → Basic → Web Security**.
- Click Edit under Trusted Sources.
- Add the value from CdnHostName (cdn.assistant.api.sitefinity.cloud) under the following sections:
	- Scripts
	- Styles
	- Images
	