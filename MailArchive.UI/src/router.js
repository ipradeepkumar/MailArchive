
import { createBrowserRouter } from "react-router-dom";
import Layout from "./pages/Shared/Layout";
import Error from "./pages/Shared/Error";
import Home from "./pages/Home";
import About from "./pages/About";
import UploadData from "./pages/uploaddata";
import MapData from "./pages/mapdata"

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Layout />,
    errorElement: <Error />,
    children: [
      { path: "/home", element: <Home /> },
      { path: "/about", element: <About /> },
      { path: "/uploaddata", element: <UploadData /> },
      { path: "/mapdata", element: <MapData /> }
    ],
  },
]);

