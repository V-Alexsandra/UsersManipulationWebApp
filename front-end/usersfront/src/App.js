import './App.css';
import { BrowserRouter, Routes, Route } from "react-router-dom"
import React from 'react';
import Registration from './Pages/Registration';
import Login from './Pages/LogIn';
import Users from './Pages/Users';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/registration" element={<Registration />} />
        <Route path="/users" element={<Users />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;