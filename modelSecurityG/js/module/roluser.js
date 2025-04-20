// roluser.js - Funcionalidad específica para la gestión de asignaciones rol-usuario

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const rolUserTable = $('#rolUserTable').DataTable({
        language: {
            // url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'id' },
            { data: 'userEmail' }, // Mostraremos el email del usuario en lugar del ID
            { data: 'rolName' },  // Mostraremos el nombre del rol en lugar del ID
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
                        <button class="btn btn-sm btn-warning edit-roluser" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-roluser" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });

    // Cargar datos iniciales
    loadUsers();
    loadRoles();
    loadRolUser();

    // Event Listeners
    document.getElementById('saveRolUser').addEventListener('click', saveRolUser);
    
    // Delegación de eventos para botones de editar y eliminar
    document.getElementById('rolUserTable').addEventListener('click', function(e) {
        if (e.target.closest('.edit-roluser')) {
            const id = e.target.closest('.edit-roluser').getAttribute('data-id');
            editRolUser(id);
        }
        
        if (e.target.closest('.delete-roluser')) {
            const id = e.target.closest('.delete-roluser').getAttribute('data-id');
            showDeleteConfirmation(id);
        }
    });

    // Limpiar modal al abrir para crear nuevo
    $('#rolUserModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('rolUserModalLabel').textContent = 'Nuevo Rol-Usuario';
            resetForm('rolUserForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteRolUser);
});

// Variables globales para almacenar usuarios y roles
let usersData = [];
let rolesData = [];

// Cargar todos los usuarios
function loadUsers() {
    return fetch(`${API_BASE_URL}/User`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los usuarios');
            }
            return response.json();
        })
        .then(data => {
            // Almacenar los datos de usuarios para uso posterior
            usersData = data;
            
            const selectUser = document.getElementById('userId');
            selectUser.innerHTML = '<option value="">Seleccionar...</option>';
            
            data.forEach(user => {
                if (!user.isDeleted && user.active) {
                    const option = document.createElement('option');
                    option.value = user.id;
                    // Mostrar solo el email para identificar al usuario
                    option.textContent = user.email;
                    selectUser.appendChild(option);
                }
            });
            
            return data; // Devolver los datos para encadenamiento de promesas si es necesario
        })
        .catch(error => {
            showNotification('Error al cargar los usuarios: ' + error.message, 'danger');
        });
}

// Cargar todos los roles
function loadRoles() {
    return fetch(`${API_BASE_URL}/Rol`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los roles');
            }
            return response.json();
        })
        .then(data => {
            // Almacenar los datos de roles para uso posterior
            rolesData = data;
            
            const selectRol = document.getElementById('rolId');
            selectRol.innerHTML = '<option value="">Seleccionar...</option>';
            
            data.forEach(rol => {
                if (!rol.isDeleted) {
                    const option = document.createElement('option');
                    option.value = rol.id;
                    // Usar el nombre del rol como texto visible
                    option.textContent = rol.name;
                    selectRol.appendChild(option);
                }
            });
            
            return data; // Devolver los datos para encadenamiento de promesas si es necesario
        })
        .catch(error => {
            showNotification('Error al cargar los roles: ' + error.message, 'danger');
        });
}

// Función auxiliar para obtener el email de usuario por ID
function getUserEmailById(userId) {
    const user = usersData.find(u => u.id === userId);
    if (user) {
        return user.email;
    }
    return `Usuario ID: ${userId}`;
}

// Función auxiliar para obtener el nombre del rol por ID
function getRolNameById(rolId) {
    const rol = rolesData.find(r => r.id === rolId);
    if (rol) {
        return rol.name;
    }
    return `Rol ID: ${rolId}`;
}

// Cargar todas las asignaciones rol-usuario
function loadRolUser() {
    // Primero aseguramos que tenemos los datos de usuarios y roles cargados
    Promise.all([
        usersData.length ? Promise.resolve(usersData) : loadUsers(),
        rolesData.length ? Promise.resolve(rolesData) : loadRoles()
    ])
    .then(() => {
        // Ahora cargamos las asignaciones rol-usuario
        return fetch(`${API_BASE_URL}/RolUser`);
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('No se pudieron cargar los datos de rol-usuario');
        }
        return response.json();
    })
    .then(data => {
        // Transformar los datos para mostrar nombres en lugar de IDs
        const transformedData = data.map(item => {
            return {
                ...item,
                userEmail: getUserEmailById(item.userId),
                rolName: getRolNameById(item.rolId)
            };
        });
        
        const table = $('#rolUserTable').DataTable();
        table.clear();
        table.rows.add(transformedData);
        table.draw();
    })
    .catch(error => {
        showNotification('Error al cargar los datos: ' + error.message, 'danger');
    });
}

// Guardar asignación rol-usuario (crear o actualizar)
function saveRolUser() {
    if (!validateForm('rolUserForm')) {
        return;
    }

    const rolUserId = document.getElementById('rolUserId').value;
    const isUpdate = rolUserId !== '' && rolUserId !== null;
    const rolUserData = serializeFormToJson('rolUserForm');
    
    const url = isUpdate ? `${API_BASE_URL}/RolUser/${rolUserId}` : `${API_BASE_URL}/RolUser`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: rolUserData
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar la asignación rol-usuario');
        }
        return response.json();
    })
    .then(data => {
        $('#rolUserModal').modal('hide');
        showNotification(isUpdate ? 'Asignación rol-usuario actualizada con éxito' : 'Asignación rol-usuario creada con éxito');
        loadRolUser();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editRolUser(id) {
    fetch(`${API_BASE_URL}/RolUser/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar la asignación rol-usuario');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('rolUserModalLabel').textContent = 'Editar Asignación Rol-Usuario';
            
            // Llenar el formulario con los datos
            document.getElementById('rolUserId').value = data.id;
            document.getElementById('userId').value = data.userId;
            document.getElementById('rolId').value = data.rolId;
            document.getElementById('isDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#rolUserModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let rolUserIdToDelete = null;

function showDeleteConfirmation(id) {
    rolUserIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar asignación rol-usuario
function deleteRolUser() {
    if (rolUserIdToDelete) {
        fetch(`${API_BASE_URL}/RolUser/${rolUserIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar la asignación rol-usuario');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Asignación rol-usuario eliminada con éxito');
            loadRolUser();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            rolUserIdToDelete = null;
        });
    }
}