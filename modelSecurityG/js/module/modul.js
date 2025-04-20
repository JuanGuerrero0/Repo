// modul.js - Funcionalidad específica para la gestión de módulos

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const modulTable = $('#modulTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'name' },
            { data: 'description' },
            { 
                data: 'Active',
                render: function(data) {
                    return data ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-danger">Inactivo</span>';
                }
            },
            { 
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-warning edit-modul" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-modul" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadModules();

    // Event Listeners
    document.getElementById('saveModul').addEventListener('click', saveModul);
    
    // Delegación de eventos para botones de editar y eliminar
    $('#modulTable').on('click', '.edit-modul', function() {
        const id = $(this).data('id');
        editModul(id);
    });
    
    $('#modulTable').on('click', '.delete-modul', function() {
        const id = $(this).data('id');
        showDeleteConfirmation(id);
    });

    // Limpiar modal al abrir para crear nuevo
    $('#modulModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('modulModalLabel').textContent = 'Nuevo Módulo';
            resetForm('modulForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteModul);
});

// Cargar todos los módulos
function loadModules() {
    fetch(`${API_BASE_URL}/Module`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#modulTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Guardar módulo (crear o actualizar)
function saveModul() {
    if (!validateForm('modulForm')) {
        return;
    }

    const modulId = document.getElementById('modulId').value;
    const isUpdate = modulId !== '' && modulId !== null;
    
    // Crear objeto con los datos del formulario
    const modulData = {
        Id: modulId ? parseInt(modulId) : 0,
        Name: document.getElementById('Name').value,
        Description: document.getElementById('Description').value,
        Active: document.getElementById('Active').checked,
        IsDeleted: document.getElementById('IsDeleted').checked
    };
    
    const url = isUpdate ? `${API_BASE_URL}/module/${modulId}` : `${API_BASE_URL}/module`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(modulData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar el módulo');
        }
        return response.json();
    })
    .then(data => {
        $('#modulModal').modal('hide');
        showNotification(isUpdate ? 'Módulo actualizado con éxito' : 'Módulo creado con éxito');
        loadModules();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editModul(id) {
    fetch(`${API_BASE_URL}/Module/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el módulo');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('modulModalLabel').textContent = 'Editar Módulo';
            
            // Llenar el formulario con los datos
            document.getElementById('modulId').value = data.id;
            document.getElementById('Name').value = data.name;
            document.getElementById('Description').value = data.description;
            document.getElementById('Active').checked = data.active;
            document.getElementById('IsDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#modulModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let modulIdToDelete = null;

function showDeleteConfirmation(id) {
    modulIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar módulo
function deleteModul() {
    if (modulIdToDelete) {
        fetch(`${API_BASE_URL}/Module/permanent/${modulIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el módulo');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Módulo eliminado con éxito');
            loadModules();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            modulIdToDelete = null;
        });
    }
}