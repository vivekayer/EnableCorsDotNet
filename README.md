# EnableCorsDotNet
A HTTP module that implements dynamic CORS support

Instructions:

1.	Copy dll to the bin folder of your service.
2.	Add the module in web config.
    <system.webServer>
      <modules>
        <add name="CorsModule" type="cScape.CIPD.Cors.CorsModule"/>
      </modules>
    </system.webServer>
3.	The module looks for an app setting that takes a comma separated list of allowed domains. For example:
<add key ="allowedOrigins" value="http://www.mysite.com, https://checkout.mysite.com"/>