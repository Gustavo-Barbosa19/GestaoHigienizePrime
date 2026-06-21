function doGet(e) {
  return handleRequest(e);
}

function doPost(e) {
  return handleRequest(e);
}

function handleRequest(e) {
  try {
    const params = e.parameter || {};
    const postData = e.postData ? JSON.parse(e.postData.contents) : {};
    
    const action = params.action || postData.action;
    const sheet = params.sheet || postData.sheet;
    
    if (!action || !sheet) {
      return sendJson({ success: false, message: 'Parâmetros action e sheet são obrigatórios' });
    }
    
    const spreadsheet = SpreadsheetApp.getActiveSpreadsheet();
    let sheetObj = spreadsheet.getSheetByName(sheet);
    
    if (!sheetObj) {
      sheetObj = spreadsheet.insertSheet(sheet);
      createHeaders(sheetObj, sheet);
    }
    
    let result;
    
    switch (action) {
      case 'getAll':
        result = getAllData(sheetObj);
        break;
      case 'getById':
        result = getById(sheetObj, params.id || postData.id);
        result = result ? [result] : [];
        break;
      case 'insert':
        result = insertData(sheetObj, postData.data || params);
        break;
      case 'update':
        result = updateData(sheetObj, params.id || postData.id, postData.data || params);
        break;
      case 'delete':
        result = deleteData(sheetObj, params.id || postData.id);
        break;
      case 'query':
        result = queryData(sheetObj, params.field, params.value);
        break;
      default:
        return sendJson({ success: false, message: 'Ação não reconhecida' });
    }
    
    return sendJson({ success: true, data: result });
    
  } catch (error) {
    return sendJson({ success: false, message: error.toString() });
  }
}

function createHeaders(sheet, sheetName) {
  const headers = {
    'Usuarios': ['Id', 'NomeUsuario', 'SenhaHash', 'Email', 'NomeCompleto', 'DataCriacao', 'Ativo'],
    'Clientes': ['Id', 'Nome', 'Telefone', 'WhatsApp', 'Email', 'CEP', 'Endereco', 'Numero', 'Bairro', 'Cidade', 'Estado', 'Observacoes', 'DataCadastro', 'Ativo'],
    'Servicos': ['Id', 'ClienteId', 'ClienteNome', 'DataAtendimento', 'Horario', 'TipoServico', 'QuantidadeItens', 'Valor', 'FormaPagamento', 'Status', 'Observacoes', 'DataCriacao'],
    'Financeiro': ['Id', 'Tipo', 'Categoria', 'Descricao', 'Valor', 'Data', 'ServicoId', 'DataCriacao'],
    'TipoServicos': ['Id', 'Nome', 'Descricao', 'Ativo']
  };
  
  const cols = headers[sheetName] || ['Id', 'Dados'];
  const headerRange = sheet.getRange(1, 1, 1, cols.length);
  headerRange.setValues([cols]);
  headerRange.setFontWeight('bold');
  headerRange.setBackground('#1a237e');
  headerRange.setFontColor('#ffffff');
}

function getAllData(sheet) {
  const data = sheet.getDataRange().getValues();
  if (data.length < 2) return [];
  
  const headers = data[0].map(h => h.toString());
  const result = [];
  
  for (let i = 1; i < data.length; i++) {
    const row = {};
    for (let j = 0; j < headers.length; j++) {
      row[toCamelCase(headers[j])] = data[i][j];
    }
    result.push(row);
  }
  
  return result;
}

function getById(sheet, id) {
  const data = sheet.getDataRange().getValues();
  if (data.length < 2) return null;
  
  const headers = data[0].map(h => h.toString());
  
  for (let i = 1; i < data.length; i++) {
    if (data[i][0] == id) {
      const row = {};
      for (let j = 0; j < headers.length; j++) {
        row[toCamelCase(headers[j])] = data[i][j];
      }
      return row;
    }
  }
  
  return null;
}

function insertData(sheet, data) {
  const headers = sheet.getDataRange().getValues()[0] || [];
  const newRow = [];
  
  for (let i = 0; i < headers.length; i++) {
    const key = toCamelCase(headers[i]);
    newRow.push(data[key] !== undefined ? data[key] : '');
  }
  
  sheet.appendRow(newRow);
  return getAllData(sheet);
}

function updateData(sheet, id, data) {
  const range = sheet.getDataRange();
  const values = range.getValues();
  if (values.length < 2) return false;
  
  const headers = values[0].map(h => h.toString());
  
  for (let i = 1; i < values.length; i++) {
    if (values[i][0] == id) {
      for (let j = 0; j < headers.length; j++) {
        const key = toCamelCase(headers[j]);
        if (data[key] !== undefined) {
          sheet.getRange(i + 1, j + 1).setValue(data[key]);
        }
      }
      return true;
    }
  }
  
  return false;
}

function deleteData(sheet, id) {
  const range = sheet.getDataRange();
  const values = range.getValues();
  
  for (let i = 1; i < values.length; i++) {
    if (values[i][0] == id) {
      sheet.deleteRow(i + 1);
      return true;
    }
  }
  
  return false;
}

function queryData(sheet, field, value) {
  const data = sheet.getDataRange().getValues();
  if (data.length < 2) return [];
  
  const headers = data[0].map(h => h.toString());
  const fieldIndex = headers.findIndex(h => toCamelCase(h) === field);
  
  if (fieldIndex === -1) return [];
  
  const result = [];
  const searchTerm = String(value).toLowerCase();
  
  for (let i = 1; i < data.length; i++) {
    const cellValue = String(data[i][fieldIndex]).toLowerCase();
    if (cellValue.includes(searchTerm)) {
      const row = {};
      for (let j = 0; j < headers.length; j++) {
        row[toCamelCase(headers[j])] = data[i][j];
      }
      result.push(row);
    }
  }
  
  return result;
}

function toCamelCase(str) {
  return str.charAt(0).toLowerCase() + str.slice(1);
}

function sendJson(data) {
  return ContentService
    .createTextOutput(JSON.stringify(data))
    .setMimeType(ContentService.MimeType.JSON);
}
