import { useEffect, useState } from "react";
import api from "../../api/axiosInstance";
import "./ProductList.css";

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  unitsInStock: number;
  discontinued: boolean;
  category: { id: number; name: string };
}

export const ProductList = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [showForm, setShowForm] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [search, setSearch] = useState("");
  const [categories, setCategories] = useState<{ id: number; name: string }[]>(
    [],
  );

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const res = await api.get("/category"); // ← coincide con tu ruta del backend
        setCategories(res.data);
      } catch (err) {
        console.error(err);
      }
    };
    fetchCategories();
  }, []);

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: 0,
    unitsInStock: 0,
    discontinued: false,
    categoryId: 1,
  });

  const pageSize = 20;

  const fetchProducts = async (pageNumber: number, searchTerm: string = "") => {
    try {
      const res = await api.get(
        `/products?page=${pageNumber}&pageSize=${pageSize}&search=${encodeURIComponent(searchTerm)}`,
      );
      setProducts(res.data.data);
      setTotalPages(Math.ceil(res.data.total / pageSize));
      setPage(pageNumber);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    fetchProducts(1);
  }, []);

  // 🔹 Buscar en tiempo real con debounce
  useEffect(() => {
    const timeout = setTimeout(() => {
      fetchProducts(1, search);
    }, 300); // 300ms después de que el usuario deje de escribir

    return () => clearTimeout(timeout);
  }, [search]);

  // Crear o actualizar producto
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
      name: formData.name,
      description: formData.description,
      price: Number(formData.price), // asegurarse que sea decimal
      unitsInStock: Number(formData.unitsInStock),
      discontinued: formData.discontinued,
      categoryId: Number(formData.categoryId),
    };

    try {
      let res;
      if (editingProduct) {
        res = await api.put(`/products/${editingProduct.id}`, payload);
      } else {
        res = await api.post("/products/create", payload); // usar solo el endpoint correcto
      }

      console.log("Producto creado/actualizado:", res.data);

      setShowForm(false);
      setEditingProduct(null);
      fetchProducts(page, search);
    } catch (err: any) {
      console.error(
        "Error creando producto:",
        err.response?.data || err.message,
      );
    }
  };

  const handleEdit = (p: Product) => {
    setEditingProduct(p);
    setFormData({
      name: p.name,
      description: p.description,
      price: p.price,
      unitsInStock: p.unitsInStock,
      discontinued: p.discontinued,
      categoryId: p.category.id,
    });
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    // eslint-disable-next-line no-restricted-globals
    if (!confirm("¿Seguro que quieres eliminar este producto?")) return;
    try {
      await api.delete(`/products/${id}`);
      fetchProducts(page, search);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="product-container">
      <div className="product-header">
        <h1>Productos</h1>
        <div className="search-bar">
          <input
            type="text"
            placeholder="Buscar por nombre..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
          <button
            onClick={() => {
              setSearch("");
              fetchProducts(1);
            }}
          >
            Limpiar
          </button>
          <button className="create-btn" onClick={() => setShowForm(true)}>
            Crear Producto
          </button>
        </div>
      </div>

      {showForm && (
        <div className="product-form">
          <h3>{editingProduct ? "Editar Producto" : "Crear Producto"}</h3>
          <form onSubmit={handleSubmit}>
            <input
              placeholder="Nombre"
              value={formData.name}
              onChange={(e) =>
                setFormData({ ...formData, name: e.target.value })
              }
              required
            />
            <input
              placeholder="Descripción"
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
            />
            <input
              type="number"
              placeholder="Precio"
              value={formData.price}
              onChange={(e) =>
                setFormData({ ...formData, price: Number(e.target.value) })
              }
              required
            />
            <input
              type="number"
              placeholder="Stock"
              value={formData.unitsInStock}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  unitsInStock: Number(e.target.value),
                })
              }
            />
            <label className="checkbox-label">
              Descontinuado
              <input
                type="checkbox"
                checked={formData.discontinued}
                onChange={(e) =>
                  setFormData({ ...formData, discontinued: e.target.checked })
                }
              />
            </label>
            <label>
              Categoría
              <select
                value={formData.categoryId}
                onChange={(e) =>
                  setFormData({
                    ...formData,
                    categoryId: Number(e.target.value),
                  })
                }
                required
              >
                <option value="">Selecciona una categoría</option>
                {categories.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>
            </label>

            <div className="form-buttons">
              <button type="submit">
                {editingProduct ? "Guardar Cambios" : "Crear"}
              </button>
              <button
                type="button"
                onClick={() => {
                  setShowForm(false);
                  setEditingProduct(null);
                }}
              >
                Cancelar
              </button>
            </div>
          </form>
        </div>
      )}

      <table className="product-table">
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Descripción</th>
            <th>Precio</th>
            <th>Stock</th>
            <th>Estado</th>
            <th>Categoría</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {products.map((p) => (
            <tr key={p.id}>
              <td>{p.name}</td>
              <td>{p.description}</td>
              <td>${p.price}</td>
              <td>{p.unitsInStock}</td>
              <td>{p.discontinued ? "Descontinuado" : "Activo"}</td>
              <td>{p.category?.name}</td>
              <td>
                <button className="edit-btn" onClick={() => handleEdit(p)}>
                  Editar
                </button>
                <button
                  className="delete-btn"
                  onClick={() => handleDelete(p.id)}
                >
                  Eliminar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div className="pagination">
        <button
          onClick={() => fetchProducts(page - 1, search)}
          disabled={page === 1}
        >
          Prev
        </button>
        <span>
          Página {page} de {totalPages}
        </span>
        <button
          onClick={() => fetchProducts(page + 1, search)}
          disabled={page === totalPages}
        >
          Next
        </button>
      </div>
    </div>
  );
};
