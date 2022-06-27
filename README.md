# Starting and Stopping Azure App Services from C#

Pre-Requisites,

1. Microsoft Azure Account - [Click here](https://azure.microsoft.com/en-in/free/)
2. Azure WebApp Service- [Click Here](https://azure.microsoft.com/en-in/services/app-service/web/)
 
### Problem:
 - We want to stop/start/Restart our web app through C# code.

### Solution:

 We are going to create a Service Principal account. The  “Service Principal” to generate the bearer tokens. After that will read all the details from the subscription.

### Steps:

1. Clone this repo https://github.com/AnbuMani27/AutomaticAzureAppServices. Here, we need the fill in the Configuration details.

     -  AzureSubscription ___Get the values from your subscription___.
     -  AzureTenantId -> Follow Below Steps
     -  AzureClientId -> Follow Below Steps
     -  AzureClientSecret -> Follow Below Steps

1. The first, we need is to create an AAD application. Go to (https://portal.azure.com/ ) and choose Active Directory in the left pane.

   ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/1.PNG)

2. Next, go to the left pane then select App registrations->New registration

   ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/2.PNG)

3.  In Register an application fill the all necessary details and click register.

    ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/3.PNG)

4. Once the applicaiton will register will get **Azure Client Id and Azure TenantId**

    ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/4.PNG)

5. Next, we need to create a Secret Value for the application. Click **Certificates & Secrets** from the left pane and Select New Client secret and fill all details for secret and then click Add.

    ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/5.PNG)

6. Once Added Secret it will generate the value for **AzureClientSecret**.

    ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/6.PNG)
 

7. Now, everything we have all the details. Finally, we need to permit as we created the AAD application. So we need to add the AAD application to our subscription. Go to Subscription=>Access control (IAM)->Add

     ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/7.PNG) 

 8. Next populated popup you need to give the role, Assign access to and select as you **created application name** in the AAD, then save.

     ![alt text](https://github.com/AnbuMani27/AutomaticAzureAppServices/blob/main/Images/8.PNG)

  Now execute your project. It will work Start and Stop base on your action.

## Happy Coding!

## References 

1. https://github.com/davidebbo/AzureWebsitesSamples/tree/master/ManagementLibrarySample
2. https://azureappservices.blogspot.com/2019/07/azure-app-service-automate.html
