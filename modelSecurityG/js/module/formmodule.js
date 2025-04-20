// formmodule.js - Funcionalidad específica para la gestión de Form-Módulo

document.addEventListener('DOMContentLoaded', function () {
    // Inicializar DataTable
    const formModuleTable = $('#formModuleTable').DataTable({
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json'
        },
        columns: [
            { data: 'formName' },
            { data: 'moduleName' },
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
                        <button class="btn btn-sm btn-warning edit-formmodule" data-id="${row.id}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-formmodule" data-id="${row.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    `;
                }
            }
        ]
    });
 
    // Cargar datos iniciales
    loadFormModules();
    loadForms();
    loadModules();

    // Event Listeners
    document.getElementById('saveFormModule').addEventListener('click', saveFormModule);
    
    // Delegación de eventos para botones de editar y eliminar
    $('#formModuleTable').on('click', '.edit-formmodule', function() {
        const id = $(this).data('id');
        editFormModule(id);
    });
    
    $('#formModuleTable').on('click', '.delete-formmodule', function() {
        const id = $(this).data('id');
        showDeleteConfirmation(id);
    });

    // Limpiar modal al abrir para crear nuevo
    $('#formModuleModal').on('show.bs.modal', function (e) {
        if (e.relatedTarget) {
            document.getElementById('formModuleModalLabel').textContent = 'Nuevo Form-Módulo';
            resetForm('formModuleForm');
        }
    });

    // Confirmación de eliminación
    document.getElementById('confirmDelete').addEventListener('click', deleteFormModule);
});

// Cargar todos los Form-Módulos
function loadFormModules() {
    fetch(`${API_BASE_URL}/FormModule`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los datos');
            }
            return response.json();
        })
        .then(data => {
            const table = $('#formModuleTable').DataTable();
            table.clear();
            table.rows.add(data);
            table.draw();
        })
        .catch(error => {
            showNotification('Error al cargar los datos: ' + error.message, 'danger');
        });
}

// Cargar todos los formularios para el select
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
            
            // Limpiar opciones existentes excepto la primera
            while (formSelect.options.length > 1) {
                formSelect.remove(1);
            }
            
            // Agregar las opciones de formularios
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

// Cargar todos los módulos para el select
function loadModules() {
    fetch(`${API_BASE_URL}/Module`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudieron cargar los módulos');
            }
            return response.json();
        })
        .then(data => {
            const moduleSelect = document.getElementById('moduleId');
            
            // Limpiar opciones existentes excepto la primera
            while (moduleSelect.options.length > 1) {
                moduleSelect.remove(1);
            }
            
            // Agregar las opciones de módulos
            data.forEach(module => {
                if (!module.isDeleted) {
                    const option = document.createElement('option');
                    option.value = module.id;
                    option.textContent = module.name;
                    moduleSelect.appendChild(option);
                }
            });
        })
        .catch(error => {
            showNotification('Error al cargar los módulos: ' + error.message, 'danger');
        });
}

// Guardar Form-Módulo (crear o actualizar)
function saveFormModule() {
    if (!validateForm('formModuleForm')) {
        return;
    }

    const formModuleId = document.getElementById('formModuleId').value;
    const isUpdate = formModuleId !== '' && formModuleId !== null;
    
    // Crear objeto con los datos del formulario
    const formModuleData = {
        id: formModuleId ? parseInt(formModuleId) : 0,
        formId: parseInt(document.getElementById('formId').value),
        moduleId: parseInt(document.getElementById('moduleId').value),
        formName: document.getElementById('formId').options[document.getElementById('formId').selectedIndex].text,
        moduleName: document.getElementById('moduleId').options[document.getElementById('moduleId').selectedIndex].text,
        isDeleted: document.getElementById('isDeleted').checked
    };
    
    const url = isUpdate ? `${API_BASE_URL}/FormModule/${formModuleId}` : `${API_BASE_URL}/FormModule`;
    const method = isUpdate ? 'PUT' : 'POST';

    fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formModuleData)
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al guardar el Form-Módulo');
        }
        return response.json();
    })
    .then(data => {
        $('#formModuleModal').modal('hide');
        showNotification(isUpdate ? 'Form-Módulo actualizado con éxito' : 'Form-Módulo creado con éxito');
        loadFormModules();
    })
    .catch(error => {
        showNotification('Error: ' + error.message, 'danger');
    });
}

// Cargar datos para edición
function editFormModule(id) {
    fetch(`${API_BASE_URL}/FormModule/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo cargar el Form-Módulo');
            }
            return response.json();
        })
        .then(data => {
            document.getElementById('formModuleModalLabel').textContent = 'Editar Form-Módulo';
            
            // Llenar el formulario con los datos
            document.getElementById('formModuleId').value = data.id;
            document.getElementById('formId').value = data.formId;
            document.getElementById('moduleId').value = data.moduleId;
            document.getElementById('isDeleted').checked = data.isDeleted;
            
            // Mostrar el modal
            $('#formModuleModal').modal('show');
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        });
}

// Preparar eliminación
let formModuleIdToDelete = null;

function showDeleteConfirmation(id) {
    formModuleIdToDelete = id;
    $('#deleteConfirmModal').modal('show');
}

// Eliminar Form-Módulo
function deleteFormModule() {
    if (formModuleIdToDelete) {
        fetch(`${API_BASE_URL}/FormModule/permanent/${formModuleIdToDelete}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('No se pudo eliminar el Form-Módulo');
            }
            $('#deleteConfirmModal').modal('hide');
            showNotification('Form-Módulo eliminado con éxito');
            loadFormModules();
        })
        .catch(error => {
            showNotification('Error: ' + error.message, 'danger');
        })
        .finally(() => {
            formModuleIdToDelete = null;
        });
    }
}