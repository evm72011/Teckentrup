FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY pub/ /root/
WORKDIR /root/
ENV ASPNETCORE_URLS="http://*:80"
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Gateway.dll"]
