# frontend/Dockerfile
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Servir con Nginx
FROM nginx:alpine
# --- LÍNEA NUEVA AQUÍ ---
COPY nginx.conf /etc/nginx/conf.d/default.conf 
# -----------------------
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]