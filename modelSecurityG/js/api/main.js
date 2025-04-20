// main.js - Funciones principales

document.addEventListener('DOMContentLoaded', function () {
    // Toggle para el sidebar
    document.getElementById('sidebarCollapse').addEventListener('click', function () {
        document.getElementById('sidebar').classList.toggle('active');
        document.getElementById('content').classList.toggle('active');
    });

    // Agregar clase active al enlace actual
    const currentPath = window.location.pathname;
    const links = document.querySelectorAll('#sidebar a');
    
    links.forEach(link => {
        if (currentPath.endsWith(link.getAttribute('href'))) {
            link.classList.add('active');
        } else {
            link.classList.remove('active');
        }
    });

    // Configuración para todas las peticiones AJAX
    $.ajaxSetup({
        headers: {
            'Content-Type': 'application/json'
        }
    });
});

// Función para mostrar notificaciones
function showNotification(message, type = 'success') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.role = 'alert';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    // Insertar alerta al principio del contenido
    const content = document.querySelector('.container-fluid');
    content.insertBefore(alertDiv, content.firstChild);
    
    // Auto-cerrar después de 3 segundos
    setTimeout(() => {
        alertDiv.classList.remove('show');
        setTimeout(() => alertDiv.remove(), 150);
    }, 3000);
}

// Función para formatear fechas
function formatDate(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString();
}

// Función para validar formularios
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return false;
    }
    return true;
}

// Función para serializar formularios a JSON
function serializeFormToJson(formId) {
    const form = document.getElementById(formId);
    const formData = new FormData(form);
    const jsonObject = {};
    
    formData.forEach((value, key) => {
        // Manejo especial para checkboxes
        if (form.elements[key].type === 'checkbox') {
            jsonObject[key] = form.elements[key].checked;
        } else {
            jsonObject[key] = value;
        }
    });
    
    return JSON.stringify(jsonObject);
}

// Función para limpiar formularios
function resetForm(formId) {
    document.getElementById(formId).reset();
    document.getElementById(formId).classList.remove('was-validated');
}

// Base URL de la API
const API_BASE_URL = 'https://localhost:7287/api'; // Ajusta según tu configuración