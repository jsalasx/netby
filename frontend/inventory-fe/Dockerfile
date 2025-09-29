# ========== Etapa 1: Build ==========
FROM node:20-alpine AS build

# Crear directorio de trabajo
WORKDIR /app

# Copiar package.json y package-lock.json primero para cachear dependencias
COPY package*.json ./

# Instalar dependencias
RUN npm ci

# Copiar todo el código fuente
COPY . .

# Construir la aplicación Angular (output en dist/)
RUN npm run build -- --configuration production

# ========== Etapa 2: Producción ==========
FROM nginx:alpine AS production

# Remover configuración default de nginx
RUN rm -rf /usr/share/nginx/html/*

# Copiar artefactos construidos de la etapa anterior
COPY --from=build /app/dist /usr/share/nginx/html

# Copiar configuración de Nginx personalizada (opcional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exponer puerto
EXPOSE 80

# Iniciar Nginx
CMD ["nginx", "-g", "daemon off;"]
