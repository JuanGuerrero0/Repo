// rol.js - Funcionalidad específica para la gestión de roles

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const rolTable = $('#rolTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'name' },
            { data: 'description' },
            { 
                data: 'isDeleted',
                render: function(data) {
                    return !data ? 
                        '<span class="badge bg-success">Activo</span>' : 
                        '<span class="badge bg-danger">Eliminado</span>';
                }
            },
            { 
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-warning edit-rol" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-rol" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadRoles();

    // Event Listeners
    document.getElementById('saveRol').addEventListener('click', saveRol);
    
    // Delegación de eventos para botones de editar y eliminar
    $('#rolTable').on('click', '.edit-rol', function() {
        const id = $(this).data('id');
        editRol(id);
    });
    
    $('#rolTable').on('click', '.delete-rol', function() {
        const id = $(this).data('id');
        showDeleteConfirmation(id);
    });

    // Limpiar modal al abrir para crear nuevo
    $('#rolModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('rolModalLabel').textContent = 'Nuevo Rol';
            resetForm('rolForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteRol);
});

// Cargar todos los roles
function loadRoles() {
    fetch(`${API_BASE_URL}/Rol`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#rolTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Guardar rol (crear o actualizar)
function saveRol() {
    if (!validateForm('rolForm')) {
        return;
    }

    const rolId = document.getElementById('Id').value;
    const isUpdate = rolId !== '' && rolId !== null;
    
    // Crear objeto con los datos del formulario
    const rolData = {
        Id: rolId ? parseInt(rolId) : 0,
        Name: document.getElementById('Name').value,
        Description: document.getElementById('Description').value,
        IsDeleted: document.getElementById('IsDeleted').checked
    };
    
    const url = isUpdate ? `${API_BASE_URL}/Rol/${rolId}` : `${API_BASE_URL}/Rol`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(rolData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar el rol');
        }
        return response.json();
    })
    .then(data => {
        $('#rolModal').modal('hide');
        showNotification(isUpdate ? 'Rol actualizado con éxito' : 'Rol creado con éxito');
        loadRoles();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editRol(id) {
    fetch(`${API_BASE_URL}/Rol/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el rol');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('rolModalLabel').textContent = 'Editar Rol';
            
            // Llenar el formulario con los datos
            document.getElementById('Id').value = data.id;
            document.getElementById('Name').value = data.Name;
            document.getElementById('Description').value = data.Description;
            document.getElementById('IsDeleted').checked = data.IsDeleted;
            
            // Mostrar el modal
            $('#rolModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let rolIdToDelete = null;

function showDeleteConfirmation(id) {
    console.log('ID para eliminar:', id);
    rolIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar rol
function deleteRol() {
    if (rolIdToDelete) {
        fetch(`${API_BASE_URL}/Rol/permanent/${rolIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el rol');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Rol eliminado con éxito');
            loadRoles();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            rolIdToDelete = null;
        });
    }
}