import { useState } from "react";
import api from "../api/axiosInstance";

export const ProductForm = () => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState<number>(0);
  const [categoryId, setCategoryId] = useState<number>(1);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await api.post("/Product", { name, description, price, categoryId });
      alert("Producto creado");
      setName(""); setDescription(""); setPrice(0); setCategoryId(1);
    } catch (err) {
      console.error(err);
      alert("Error al crear producto");
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Nombre" />
      <input value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Descripción" />
      <input type="number" value={price} onChange={(e) => setPrice(Number(e.target.value))} placeholder="Precio" />
      <select value={categoryId} onChange={(e) => setCategoryId(Number(e.target.value))}>
        <option value={1}>SERVIDORES</option>
        <option value={2}>CLOUD</option>
      </select>
      <button type="submit">Crear Producto</button>
    </form>
  );
};
