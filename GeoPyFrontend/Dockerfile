FROM node:20-alpine AS build

WORKDIR /app
COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

FROM node:20-alpine
WORKDIR /app

COPY --from=build /app /app

RUN npm install -g vite

EXPOSE 5173
CMD ["vite", "preview", "--port", "5173", "--host"]
