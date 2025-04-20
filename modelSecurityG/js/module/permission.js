// permission.js - Funcionalidad específica para la gestión de permisos

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const permissionTable = $('#permissionTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'name' },
            { data: 'description' },
            { 
                data: 'status',
                render: function(data) {
                    return data === 1 ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-danger">Inactivo</span>';
                }
            },
            { 
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-warning edit-permission" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-permission" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadPermissions();

    // Event Listeners
    document.getElementById('savePermission').addEventListener('click', savePermission);
    
    // Delegación de eventos para botones de editar y eliminar
    $('#permissionTable').on('click', '.edit-permission', function() {
        const id = $(this).data('id');
        editPermission(id);
    });
    
    $('#permissionTable').on('click', '.delete-permission', function() {
        const id = $(this).data('id');
        showDeleteConfirmation(id);
    });

    // Limpiar modal al abrir para crear nuevo
    $('#permissionModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('permissionModalLabel').textContent = 'Nuevo Permiso';
            resetForm('permissionForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deletePermission);
});

// Cargar todos los permisos
function loadPermissions() {
    fetch(`${API_BASE_URL}/Permission`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#permissionTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Guardar permiso (crear o actualizar)
function savePermission() {
    if (!validateForm('permissionForm')) {
        return;
    }

    const permissionId = document.getElementById('Id').value;
    const isUpdate = permissionId !== '' && permissionId !== null;
    
    // Crear objeto con los datos del formulario
    const permissionData = {
        Id: permissionId ? parseInt(permissionId) : 0,
        Name: document.getElementById('Name').value,
        Description: document.getElementById('Description').value,
        Status: parseInt(document.getElementById('Status').value)
    };
    
    const url = isUpdate ? `${API_BASE_URL}/Permission/${permissionId}` : `${API_BASE_URL}/Permission`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(permissionData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar el permiso');
        }
        return response.json();
    })
    .then(data => {
        $('#permissionModal').modal('hide');
        showNotification(isUpdate ? 'Permiso actualizado con éxito' : 'Permiso creado con éxito');
        loadPermissions();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editPermission(id) {
    fetch(`${API_BASE_URL}/Permission/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el permiso');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('permissionModalLabel').textContent = 'Editar Permiso';
            
            // Llenar el formulario con los datos
            document.getElementById('Id').value = data.id;
            document.getElementById('Name').value = data.name;
            document.getElementById('Description').value = data.description;
            document.getElementById('Status').value = data.status;
        
            
            // Mostrar el modal
            $('#permissionModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let permissionIdToDelete = null;

function showDeleteConfirmation(id) {
    permissionIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar permiso
function deletePermission() {
    if (permissionIdToDelete) {
        fetch(`${API_BASE_URL}/Permission/permanent/${permissionIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el permiso');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Permiso eliminado con éxito');
            loadPermissions();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            permissionIdToDelete = null;
        });
    }
}