# Etapa 1: Build Angular
FROM node:20-alpine as builder

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build

# Etapa 2: Nginx
FROM nginx:alpine

COPY --from=builder /app/dist/integrador-front /usr/share/nginx/html

# Arquivo de configuração customizado (se necessário)
COPY nginx.conf /etc/nginx/conf.d/default.conf
