// rolformpermission.js - Funcionalidad específica para la gestión de Rol-Formulario-Permiso

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const rolFormPermissionTable = $('#rolFormPermissionTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'rolName' },
            { data: 'formName' },
            { data: 'permissionName' },
            { 
                data: 'isDeleted',
                render: function(data) {
                    return data ? 
                        '<span class="badge bg-danger">Eliminado</span>' : 
                        '<span class="badge bg-success">Activo</span>';
                }
            },
            { 
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-warning edit-rolformpermission" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-rolformpermission" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadRolFormPermissions();
    
    // Cargar datos para los select
    loadRoles();
    loadForms();
    loadPermissions();

    // Event Listeners
    document.getElementById('saveRolFormPermission').addEventListener('click', saveRolFormPermission);
    
    // Delegación de eventos para botones de editar y eliminar
    $('#rolFormPermissionTable').on('click', '.edit-rolformpermission', function() {
        const id = $(this).data('id');
        editRolFormPermission(id);
    });
    
    $('#rolFormPermissionTable').on('click', '.delete-rolformpermission', function() {
        const id = $(this).data('id');
        showDeleteConfirmation(id);
    });

    // Limpiar modal al abrir para crear nuevo
    $('#rolFormPermissionModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('rolFormPermissionModalLabel').textContent = 'Nuevo Rol-Form-Permiso';
            resetForm('rolFormPermissionForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteRolFormPermission);
});

// Cargar todos los Rol-Form-Permisos
function loadRolFormPermissions() {
    fetch(`${API_BASE_URL}/RolFormPermission`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#rolFormPermissionTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Cargar roles para el select
function loadRoles() {
    fetch(`${API_BASE_URL}/Rol`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los roles');
            }
            return response.json();
        })
        .then(data => {
            const rolSelect = document.getElementById('rolId');
            rolSelect.innerHTML = '<option value="">Seleccionar...</option>';
            
            data.forEach(rol => {
                if (!rol.isDeleted) {
                    const option = document.createElement('option');
                    option.value = rol.id;
                    option.textContent = rol.name;
                    rolSelect.appendChild(option);
                }
            });
        })
        .catch(error => {
            showNotification('Error al cargar los roles: ' + error.message, 'danger');
        });
}

// Cargar formularios para el select
function loadForms() {
    fetch(`${API_BASE_URL}/Form`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los formularios');
            }
            return response.json();
        })
        .then(data => {
            const formSelect = document.getElementById('formId');
            formSelect.innerHTML = '<option value="">Seleccionar...</option>';
            
            data.forEach(form => {
                if (!form.isDeleted) {
                    const option = document.createElement('option');
                    option.value = form.id;
                    option.textContent = form.name;
                    formSelect.appendChild(option);
                }
            });
        })
        .catch(error => {
            showNotification('Error al cargar los formularios: ' + error.message, 'danger');
        });
}

// Cargar permisos para el select
function loadPermissions() {
    fetch(`${API_BASE_URL}/Permission`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los permisos');
            }
            return response.json();
        })
        .then(data => {
            const permissionSelect = document.getElementById('permissionId');
            permissionSelect.innerHTML = '<option value="">Seleccionar...</option>';
            
            data.forEach(permission => {
                if (!permission.isDeleted) {
                    const option = document.createElement('option');
                    option.value = permission.id;
                    option.textContent = permission.name;
                    permissionSelect.appendChild(option);
                }
            });
        })
        .catch(error => {
            showNotification('Error al cargar los permisos: ' + error.message, 'danger');
        });
}

// Guardar Rol-Form-Permiso (crear o actualizar)
function saveRolFormPermission() {
    if (!validateForm('rolFormPermissionForm')) {
        return;
    }

    const rolFormPermissionId = document.getElementById('rolFormPermissionId').value;
    const isUpdate = rolFormPermissionId !== '' && rolFormPermissionId !== null;
    
    // Crear objeto con los datos del formulario
    const formData = {
        id: isUpdate ? parseInt(rolFormPermissionId) : 0,
        rolId: parseInt(document.getElementById('rolId').value),
        formId: parseInt(document.getElementById('formId').value),
        permissionId: parseInt(document.getElementById('permissionId').value),
        isDeleted: document.getElementById('isDeleted').checked
    };
    
    const url = isUpdate ? `${API_BASE_URL}/RolFormPermission/${rolFormPermissionId}` : `${API_BASE_URL}/RolFormPermission`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar el Rol-Form-Permiso');
        }
        return response.json();
    })
    .then(data => {
        $('#rolFormPermissionModal').modal('hide');
        showNotification(isUpdate ? 'Rol-Form-Permiso actualizado con éxito' : 'Rol-Form-Permiso creado con éxito');
        loadRolFormPermissions();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editRolFormPermission(id) {
    fetch(`${API_BASE_URL}/RolFormPermission/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el Rol-Form-Permiso');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('rolFormPermissionModalLabel').textContent = 'Editar Rol-Form-Permiso';
            
            // Llenar el formulario con los datos
            document.getElementById('rolFormPermissionId').value = data.id;
            document.getElementById('rolId').value = data.rolId;
            document.getElementById('formId').value = data.formId;
            document.getElementById('permissionId').value = data.permissionId;
            document.getElementById('isDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#rolFormPermissionModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let rolFormPermissionIdToDelete = null;

function showDeleteConfirmation(id) {
    rolFormPermissionIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar Rol-Form-Permiso
function deleteRolFormPermission() {
    if (rolFormPermissionIdToDelete) {
        fetch(`${API_BASE_URL}/RolFormPermission/${rolFormPermissionIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el Rol-Form-Permiso');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Rol-Form-Permiso eliminado con éxito');
            loadRolFormPermissions();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            rolFormPermissionIdToDelete = null;
        });
    }
}