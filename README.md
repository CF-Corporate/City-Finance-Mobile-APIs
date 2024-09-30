To run this project

docker build -t cf-mobile-api .

docker run -d --name cf-mobile-api-container -p 7278:7278 cf-mobile-api
