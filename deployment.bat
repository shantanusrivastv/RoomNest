kubectl delete deploy roomnest-api
REM Api image

cd RoomNest.API\
dotnet publish --framework net8.0 --os linux  -p:PublishProfile=DefaultContainer
cd ..
kubectl apply -f deployment.yaml
