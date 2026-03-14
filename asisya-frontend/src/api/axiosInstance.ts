// src/api/axiosInstance.ts
import axios from "axios";

const baseURL = "http://localhost:5000/api";

const axiosInstance = axios.create({ baseURL, headers: { "Content-Type": "application/json" } });

// Interceptor para añadir token a cada request
axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default axiosInstance;
