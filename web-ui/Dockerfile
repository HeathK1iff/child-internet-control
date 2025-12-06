ARG AUTH_KEY
ARG API_URL

FROM node:18-alpine
ARG AUTH_KEY
ARG API_URL
WORKDIR /app
COPY package*.json ./
COPY ./public ./public
COPY ./src ./src
ENV REACT_APP_AUTH_KEY=${AUTH_KEY}
ENV REACT_APP_API_URL=${API_URL}
RUN npm install
CMD ["npm", "run", "start"]