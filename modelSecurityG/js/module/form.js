// form.js - Funcionalidad específica para la gestión de formularios

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const formTable = $('#formTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'name' },
            { data: 'description' },
            { data: 'url' },
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
                        <button class="btn btn-sm btn-warning edit-form" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-form" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadForms();

    // Event Listeners
    document.getElementById('saveForm').addEventListener('click', saveForm);
    
    // Delegación de eventos para botones de editar y eliminar
    $('#formTable').on('click', '.edit-form', function() {
        const id = $(this).data('id');
        editForm(id);
    });
    
    $('#formTable').on('click', '.delete-form', function() {
        const id = $(this).data('id');
        showDeleteConfirmation(id);
    });

    // Limpiar modal al abrir para crear nuevo
    $('#formModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('formModalLabel').textContent = 'Nuevo Formulario';
            resetForm('formForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteForm);
});

// Cargar todos los formularios
function loadForms() {
    fetch(`${API_BASE_URL}/Form`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#formTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Guardar formulario (crear o actualizar)
function saveForm() {
    if (!validateForm('formForm')) {
        return;
    }

    const formId = document.getElementById('formId').value;
    const isUpdate = formId !== '' && formId !== null;
    
    // Crear objeto con los datos del formulario
    const formData = {
        id: formId ? parseInt(formId) : 0,
        name: document.getElementById('name').value,
        description: document.getElementById('description').value,
        url: document.getElementById('url').value,
        isDeleted: document.getElementById('isDeleted').checked
    };
    
    const url = isUpdate ? `${API_BASE_URL}/Form/${formId}` : `${API_BASE_URL}/Form`;
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
            throw new Error('Error al guardar el formulario');
        }
        return response.json();
    })
    .then(data => {
        $('#formModal').modal('hide');
        showNotification(isUpdate ? 'Formulario actualizado con éxito' : 'Formulario creado con éxito');
        loadForms();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editForm(id) {
    fetch(`${API_BASE_URL}/Form/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el formulario');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('formModalLabel').textContent = 'Editar Formulario';
            
            // Llenar el formulario con los datos
            document.getElementById('formId').value = data.id;
            document.getElementById('name').value = data.name;
            document.getElementById('description').value = data.description;
            document.getElementById('url').value = data.url;
            document.getElementById('isDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#formModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let formIdToDelete = null;

function showDeleteConfirmation(id) {
    formIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar formulario
function deleteForm() {
    if (formIdToDelete) {
        fetch(`${API_BASE_URL}/Form/permanent/${formIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el formulario');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Formulario eliminado con éxito');
            loadForms();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            formIdToDelete = null;
        });
    }
}