<template>
  <div class="projects-container">
    <div class="header">
      <h2>專案列表test</h2>
      <el-button type="primary" @click="showCreateDialog = true"> 新增專案 </el-button>
    </div>

    <el-table :data="projects" style="width: 100%">
      <el-table-column prop="name" label="專案名稱" />
      <el-table-column prop="description" label="描述" />
      <el-table-column prop="status" label="狀態" width="100" />
      <el-table-column label="操作" width="200">
        <template #default="scope">
          <el-button size="small" @click="goToProject(scope.row.id)"> 查看 </el-button>

          <el-button
            size="small"
            type="danger"
            :loading="deletingProjectId === scope.row.id"
            :disabled="deletingProjectId === scope.row.id"
            @click="deleteProject(scope.row)"
          >
            刪除
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <!-- 新增專案對話框 -->
    <el-dialog v-model="showCreateDialog" title="新增專案" width="400px">
      <el-form ref="projectFormRef" :model="newProject" :rules="projectRules">
        <el-form-item label="名稱" prop="name">
          <el-input v-model="newProject.name" />
        </el-form-item>
        <el-form-item label="描述" prop="description">
          <el-input
            v-model="newProject.description"
            type="textarea"
            maxlength="500"
            show-word-limit
          />
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showCreateDialog = false">取消</el-button>
        <el-button type="primary" @click="createProject">確認</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from '../api'

const router = useRouter()
const projects = ref([])
const showCreateDialog = ref(false)
const newProject = ref({ name: '', description: '' })
const deletingProjectId = ref(null)

const fetchProjects = async () => {
  const response = await api.get('/api/project')
  projects.value = response.data
}

const createProject = async () => {
  if (!projectFormRef.value) return

  const valid = await projectFormRef.value.validate()

  if (!valid) return

  try {
    await api.post('/api/project', newProject.value)

    ElMessage.success('專案建立成功')

    showCreateDialog.value = false

    newProject.value = {
      name: '',
      description: ''
    }

    await fetchProjects()
  } catch (error) {
    ElMessage.error('專案建立失敗')
  }
}
const deleteProject = async (project) => {
  try {
    await ElMessageBox.confirm(`確定要刪除「${project.name}」嗎？`, '刪除專案', {
      confirmButtonText: '刪除',
      cancelButtonText: '取消',
      type: 'warning',
    })

    deletingProjectId.value = project.id

    await api.delete(`/api/project/${project.id}`)

    ElMessage.success('專案刪除成功')

    await fetchProjects()
  } catch (error) {
    if (error === 'cancel' || error === 'close') {
      return
    }

    ElMessage.error('專案刪除失敗')
  } finally {
    deletingProjectId.value = null
  }
}
const projectRules = {
  name: [
    { required: true, message: '請輸入專案名稱', trigger: 'blur' },
    { min: 2, max: 100, message: '名稱需為 2～100 個字元', trigger: 'blur' },
  ],
  description: [{ max: 500, message: '描述最多 500 個字元', trigger: 'blur' }],
}
const projectFormRef = ref()
const goToProject = (id) => {
  router.push(`/projects/${id}`)
}

onMounted(() => {
  fetchProjects()
})
</script>

<style scoped>
.projects-container {
  padding: 20px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}
</style>
