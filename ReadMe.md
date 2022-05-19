# Auxeltus

Auxeltus is a platform for providing human resources functionality. It is (obviously), currently in very early development, with the following features targeted:

- **Personal Profile**: Manage your personal information, with visibility settings.
- **Position Heirarchical View**: Navigate through the reporting structure in a clear and succinct manner.
- **Internal Job Postings**: View (and get recommended?) internal job postings.
- **Communication**: Send and receive communications natively within the platform; no need for email (just kidding you obviously would still need an email).
- **ToDo / Action Items**: Complete tasks assigned to you by the organization.
- **Time Management**: Submit your time card and schedule your absences and time off.

Additional functionality inextricably linked to money (payslips and expense reporting) may be stubbed out, but are unlikely to ever have full implementations. Other features may find their way in as well, you never know.

## Stack

Auxeltus will be composed of a web of multiple applications, and final architecture is still under construction. Current design involves:
- **Auxeltus API**: The underlying RESTful JSON API that powers the functionality, built with ASP .Net Core 3.1 and deployed through Azure.
- **Auxeltus UI**: The application that users will actually interact with, built with a ReactJS front-end, also deployed through Azure.
- **Auxeltus Processor**: The back-end application that will execute various scheduled tasks to ensure data is in the correct state. Also built on a .Net Core stack and deployed in Azure.

## Testing

Yes, Virginia, there will be testing. Unit tests for the access layers, the API, and the front-end, written using MSTest for the C# code and Mocha for the JS/TS code. As the applications advance, integration and UI testing may show up as well, and get brought into the DevOps pipeline. Early days.