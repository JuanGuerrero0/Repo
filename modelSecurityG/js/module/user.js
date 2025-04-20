// user.js - Funcionalidad específica para la gestión de usuarios

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const userTable = $('#userTable').DataTable({
        language: {
            // url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'id' },
            { data: 'email' },
            { data: 'namePerson' },
            { 
                data: 'active',
                render: function(data) {
                    return data ? 
                        '<span class="badge bg-success">Activo</span>' : 
                        '<span class="badge bg-danger">Inactivo</span>';
                }
            },
            { 
                data: 'registrationDate',
                render: function(data) {
                    return formatDate(data);
                }
            },
            { 
                data: null,
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-warning edit-user" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-user" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });

    // Cargar datos iniciales
    loadUser();

    // Event Listeners
    document.getElementById('saveUser').addEventListener('click', saveUser);
    
    // Delegación de eventos para botones de editar y eliminar
    document.getElementById('userTable').addEventListener('click', function(e) {
        if (e.target.closest('.edit-user')) {
            const id = e.target.closest('.edit-user').getAttribute('data-id');
            editUser(id);
        }
        
        if (e.target.closest('.delete-user')) {
            const id = e.target.closest('.delete-user').getAttribute('data-id');
            showDeleteConfirmation(id);
        }
    });

    // Limpiar modal al abrir para crear nuevo
    $('#userModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('userModalLabel').textContent = 'Nuevo Usuario';
            resetForm('userForm');
            document.getElementById('active').checked = true;
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteUser);
});

// Cargar todos los usuarios
function loadUser() {
    fetch(`${API_BASE_URL}/User`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#userTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Cargar personas para select
function loadUsers() {
    fetch(`${API_BASE_URL}/User`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar las personas');
            }
            return response.json();
        })
        .then(data => {
            const selectPerson = document.getElementById('personId');
            selectPerson.innerHTML = '<option value="">Seleccionar...</option>';
            
            data.forEach(person => {
                if (!person.isDeleted) {
                    const option = document.createElement('option');
                    option.value = person.id;
                    option.textContent = `${person.firstName} ${person.lastName} - ${person.document}`;
                    selectPerson.appendChild(option);
                }
            });
        })
        .catch(error => {
            showNotification('Error al cargar las personas: ' + error.message, 'danger');
        });
}

// Guardar usuario (crear o actualizar)
function saveUser() {
    if (!validateForm('userForm')) {
        return;
    }

    const userId = document.getElementById('userId').value;
    const isUpdate = userId !== '' && userId !== null;
    const userData = serializeFormToJson('userForm');
    
    const url = isUpdate ? `${API_BASE_URL}/User/${userId}` : `${API_BASE_URL}/User`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: userData
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar el usuario');
        }
        return response.json();
    })
    .then(data => {
        $('#userModal').modal('hide');
        showNotification(isUpdate ? 'Usuario actualizado con éxito' : 'Usuario creado con éxito');
        loadUser();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editUser(id) {
    fetch(`${API_BASE_URL}/User/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el usuario');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('userModalLabel').textContent = 'Editar Usuario';
            
            // Llenar el formulario con los datos
            document.getElementById('userId').value = data.id;
            document.getElementById('email').value = data.email;
            // No cargamos la contraseña por seguridad, se pedirá nueva si se quiere cambiar
            document.getElementById('personId').value = data.personId;
            document.getElementById('active').checked = data.active;
            document.getElementById('isDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#userModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let userIdToDelete = null;

function showDeleteConfirmation(id) {
    userIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar usuario
function deleteUser() {
    if (userIdToDelete) {
        fetch(`${API_BASE_URL}/User/${userIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el usuario');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Usuario eliminado con éxito');
            loadUser();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            userIdToDelete = null;
        });
    }
}