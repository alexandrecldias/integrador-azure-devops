# Etapa 1: build da aplicação
FROM node:20.19-alpine as build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build --prod

# Etapa 2: imagem final com NGINX
FROM nginx:alpine

# Garante que os logs existam
RUN mkdir -p /var/log/nginx && touch /var/log/nginx/access.log /var/log/nginx/error.log

# Remove a configuração default do NGINX
RUN rm -rf /usr/share/nginx/html/*

# Copia os arquivos compilados do Angular para a pasta servida pelo NGINX
COPY --from=build /app/dist/integrador-front/browser /usr/share/nginx/html

# Copia um arquivo customizado de configuração do NGINX (se necessário)
COPY nginx.conf /etc/nginx/nginx.conf


EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
